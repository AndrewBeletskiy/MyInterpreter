using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InterpreterForm
{
    static class RelationTableCreator
    {
        public static List<Rule> Rules;
        static List<Relation> EqualsPairs;
        static List<Relation> LessPairs;
        static List<Relation> MorePairs;
        static List<Relation> Relations;    
        public static List<Relation> FindRelations()
        {
            ReadRules();

            FindEqualsPairs();
            FindLessPairs();
            FindMorePairs();
            List<Relation> relations = new List<Relation>();


            relations.AddRange(EqualsPairs);
            relations.AddRange(LessPairs);
            relations.AddRange(MorePairs);

            Test(relations);
            Relations = relations;
            return Relations;
        }

        static void Test(List<Relation> relation)
        {
            HashSet<Relation> set = new HashSet<Relation>();
            foreach (var item in relation)
            {
                set.Add(item);
            }
            relation.Clear();
            relation.AddRange(set);
            relation.Sort((first, second) =>
            {
                return first.ToString().CompareTo(second.ToString());
            });
            var relation2 = new List<Relation>();
            for (var i = 0; i < relation.Count; i++)
            {
                if (i == relation.Count - 1)
                {
                    relation2.Add(relation[i]);
                    break;
                }
                if (!relation[i].Equals(relation[i + 1]))
                    relation2.Add(relation[i]);
            }
            for (var i = 0; i < relation2.Count - 1; i++)
            {
                if (relation2[i].First == relation2[i + 1].First && relation2[i].Second == relation2[i + 1].Second)
                {
                    throw new Exception("ERROR:" + relation[i] + relation[i + 1]);
                }
            }
        }

        static void ReadRules()
        {
            List<string> rulesString = File.ReadAllLines("rules.txt").ToList<string>();
            Rules = new List<Rule>(rulesString.Count);
            foreach (string ruleString in rulesString)
            {
                if (!String.IsNullOrEmpty(ruleString))
                {
                    List<string> split1 = ruleString.Split(" ::= ");
                    Rule newRule = new Rule(split1[0]);
                    List<string> split2 = split1[1].Split(" ");
                    newRule.Description.AddRange(split2);
                    Rules.Add(newRule);
                }
            }
        }

        static void FindEqualsPairs()
        {
            EqualsPairs = new List<Relation>(Rules.Sum(rule => rule.Count - 1));
            foreach (Rule rule in Rules)
            {
                int count = rule.Count;
                if (count > 1)
                {
                    for (var i = 0; i < count - 1; i++)
                    {
                        EqualsPairs.Add(new Relation(rule[i], rule[i + 1], RelationEnum.Equal));
                    }
                }
            }
            Comparison<Relation> comparison = new Comparison<Relation>((f, s) =>
            {
                return String.Compare(f.First + f.Second, s.First + s.Second);
            });
            EqualsPairs.Sort(comparison);

        }
        static void FindLessPairs()
        {
            LessPairs = new List<Relation>();
            List<Relation> secondNotTerminal = EqualsPairs.Where(elem => elem.Second.IsNotTerminal())
                                                          .ToList<Relation>();
            foreach (var rel in secondNotTerminal)
            {
                HashSet<string> firstPlus = FirstPlus(rel.Second);
                foreach (var second in firstPlus)
                {
                    LessPairs.Add(new Relation(rel.First, second, RelationEnum.Less));
                }

            }
        }
        static void FindMorePairs()
        {
            MorePairs = new List<Relation>();
            List<Relation> firstNotTerminal = EqualsPairs.Where(elem => elem.First.IsNotTerminal())
                                                         .ToList<Relation>();
            foreach (var rel in firstNotTerminal)
            {

                if (rel.Second.IsNotTerminal())
                {
                    string W = rel.First;
                    string V = rel.Second;
                    var last = LastPlus(W);
                    var first = FirstPlus(V);
                    foreach (var R in last)
                    {
                        foreach (var S in first)
                        {
                            MorePairs.Add(new Relation(R, S, RelationEnum.Greater));
                        }
                    }

                }
                else
                {

                    string W = rel.First;
                    string S = rel.Second;
                    var last = LastPlus(W);
                    foreach (var R in last)
                    {
                        MorePairs.Add(new Relation(R, S, RelationEnum.Greater));
                    }
                }
            }
        }
        static HashSet<string> FirstPlus(string head)
        {
            HashSet<string> set = new HashSet<string>();
            return FirstPlus(head, set);
        }
        static HashSet<string> FirstPlus(string head, HashSet<string> founded)
        {
            foreach (var rule in FindRules(head))
            {
                if (!founded.Contains(rule[0]))
                {
                    founded.Add(rule[0]);
                    if (rule[0].IsNotTerminal())
                    {
                        foreach (var item in FirstPlus(rule[0], founded))
                        {
                            founded.Add(item);
                        }
                    }
                }
            }
            return founded;
        }
        static HashSet<string> LastPlus(string head)
        {
            HashSet<string> set = new HashSet<string>();
            return LastPlus(head, set);
        }
        static HashSet<string> LastPlus(string head, HashSet<string> founded)
        {
            foreach (var rule in FindRules(head))
            {
                if (!founded.Contains(rule[rule.Count - 1]))
                {
                    founded.Add(rule[rule.Count - 1]);
                    if (rule[rule.Count - 1].IsNotTerminal())
                    {
                        foreach (var item in LastPlus(rule[rule.Count - 1], founded))
                        {
                            founded.Add(item);
                        }
                    }
                }
            }
            return founded;
        }
        static List<Rule> FindRules(string head)
        {
            return Rules.Where(elem => elem.Head.Equals(head)).ToList<Rule>();
        }
    }
    class Rule
    {
        public string Head { get; }
        public List<string> Description { get; }
        static List<string> order = new List<string>() { "read", "write", "if", "then", "else", "do", "while", "end", "or", "not", "and", "{", "}", "(", ")", "[", "]", "+", "-", "*", "/", ";", ",", ":=", "=", ">", "<", ">=", "<=", "!=", "ід", "константа", "рядок", "<прог>", "<сп.оп1>", "<сп.оп>", "<оператор>", "<ввод>", "<сп.ідент1>", "<сп.ідент>", "<вивод>", "<сп.виводу1>", "<сп.виводу>", "<елем.виводу>", "<присв>", "<вир1>", "<вир>", "<терм1>", "<терм>", "<множ>", "<ум.перехід>", "<лог.вир1>", "<лог.вир>", "<лог.терм1>", "<лог.терм>", "<лог.множ>", "<відношення>", "<знак-порівняння>", "<цикл>" };
        public string this[int i]
        {
            get
            {
                return Description[i];
            }
        }
        public Rule(string Head)
        {
            this.Head = Head;
            Description = new List<string>(1);
        }
        public int Count { get { return Description.Count; } }
        public override string ToString()
        {
            StringBuilder build = new StringBuilder();
            build.Append(String.Format("{0, -20}::=", Head));
            foreach (var desc in Description)
            {
                build.Append('\t').Append(desc);
            }
            return build.ToString();
        }

    }
    class Relation
    {
        public string First { get; }
        public string Second { get; }
        public RelationEnum State { get; }
        public Relation(string First, string Second, RelationEnum Relation)
        {
            this.First = First;
            this.Second = Second;
            this.State = Relation;
        }
        public override string ToString()
        {
            string separator = null;
            switch (State)
            {
                case RelationEnum.Greater:
                    separator = ">";
                    break;
                case RelationEnum.Equal:
                    separator = "=";
                    break;
                case RelationEnum.Less:
                    separator = "<";
                    break;
                default:
                    separator = "not in relation";
                    break;
            }
            return String.Format("{0}\t{1}\t{2}", First, separator, Second);
        }
        public override int GetHashCode()
        {
            return First.Length + Second.Length;
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is Relation))
                return false;
            var rel = obj as Relation;
            return First == rel.First && Second == rel.Second && State == rel.State;
        }
    }
    enum RelationEnum { Greater, Equal, Less, Empty }

    static class StringHelper
    {
        public static List<string> Split(this String str, String separator)
        {
            List<string> res = new List<string>();
            while (!str.Equals(""))
            {
                int index = str.IndexOf(separator);
                if (index >= 0)
                {
                    res.Add(str.Substring(0, index));
                    str = str.Remove(0, index + separator.Length);
                }
                else
                {
                    res.Add(str);
                    str = "";
                }
            }
            return res;
        }
        public static bool IsNotTerminal(this string item)
        {
            return (item[0] == '<') && (item.Last() == '>');
        }
        public static string GetSign(this RelationEnum State)
        {
            switch (State)
            {
                case RelationEnum.Greater:
                    return ">";
                case RelationEnum.Equal:
                    return "=";
                case RelationEnum.Less:
                    return "<";
                default:
                    return "!!ERROR!!";
            }
        }
    }
}
