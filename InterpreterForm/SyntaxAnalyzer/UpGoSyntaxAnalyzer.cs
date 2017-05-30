using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    class UpGoSyntaxAnalyzer : ISyntaxAnalyzer
    {
        LexemeAnalyzerOutTable testedTable;
        List<Relation> relations;
        public List<LogElement> Log;
        List<Rule> rules;
        Queue<UpGoElement> input;
        public int ErrorLine { get; private set; }
        public UpGoSyntaxAnalyzer()
        {
            ErrorLine = -1;
            Log = new List<LogElement>();

        }
        public string ErrorMessage
        {
            get; private set;
        }

        public bool Test()
        {
            ErrorLine = -1;
            Log.Clear();
            try
            {
                relations = RelationTableCreator.FindRelations();
                input = new Queue<UpGoElement>();
                rules = RelationTableCreator.Rules;
                for (var i = 0; i < testedTable.Length; i++)
                {
                    var elem = testedTable[i];
                    input.Enqueue(new UpGoElement(elem.Substring, elem.LexemeCode, elem.StringNumber));
                }
                

                Stack<UpGoElement> stack = new Stack<UpGoElement>();
                stack.Push(new UpGoElement("#", -1, 0));
                input.Enqueue(new UpGoElement("#", -1, input.Last().LineNumber));
                var IsEnd = false;
                var step = 0;
                while (!IsEnd)
                {
                    if (step >= 1e6)
                    {
                        ErrorMessage = "Синтаксическая ошибка";
                        return false;
                    }
                    RelationEnum rel;
                    try
                    {
                        rel = UpGoElement.GetRelation(stack.Peek(), input.Peek(), relations);
                    } catch (Exception ex)
                    {
                        SaveStateToLog(stack, new Stack<UpGoElement>(), RelationEnum.Empty, input);
                        ErrorLine = 0;
                        throw ex;
                    }
                    
                    
                    switch (rel)
                    {
                        case RelationEnum.Greater:
                            Stack<UpGoElement> baseStack = new Stack<UpGoElement>();
                            do
                            {
                                baseStack.Push(stack.Pop());
                            } while (UpGoElement.GetRelation(stack.Peek(), baseStack.Peek(), relations) != RelationEnum.Less);
                            UpGoElement newElement = FindElement(baseStack);
                            SaveStateToLog(stack, baseStack, rel, input);
                            stack.Push(newElement);
                            if (stack.Count + input.Count == 3)
                            {
                                if (rules[0].Head == stack.Peek().Value)
                                {
                                    SaveStateToLog(stack, new Stack<UpGoElement>(), RelationEnum.Greater, input);
                                    return true;
                                }
                            }
                            break;
                        case RelationEnum.Equal:
                        case RelationEnum.Less:
                            SaveStateToLog(stack, new Stack<UpGoElement>(), rel, input);
                            stack.Push(input.Dequeue());
                            break;
                        case RelationEnum.Empty:
                            return false;

                    }
                }
            }
            catch (LexicalError err)
            {
                ErrorMessage = err.Message;
                ErrorLine = err.Line;
                return false;
            }
            catch (Exception ex)
            {
                ErrorLine = 0;
                ErrorMessage = ex.Message;
                return false;     
            }
            return true;
        }
        public bool Test(LexemeAnalyzerOutTable table)
        {
            this.testedTable = table;
            return Test();
        }

        public UpGoElement FindElement(Stack<UpGoElement> baseElements)
        {
            List<UpGoElement> rightPart = new List<UpGoElement>();
            
            while (baseElements.Count >= 1)
            {
                rightPart.Add(baseElements.Pop());
            }
            for (var i = rightPart.Count-1; i >= 0; i--)
            {
                baseElements.Push(rightPart[i]);
            }
            var rule = rules.Find(r => {
                if (r.Description.Count != rightPart.Count)
                {
                    return false;
                }
                for (var i = 0; i < r.Description.Count; i++)
                {
                    if (!UpGoElement.CompareValues(rightPart[i], r.Description[i]))
                    {
                        return false;
                    }
                }

                return true;
            });
            if (rule == null)
            {
                var rightPartString = "";
                foreach (var elem in rightPart)
                {
                    rightPartString += elem.Value;
                }
                throw new LexicalError($"Ошибка: Нет такой основы: {rightPartString}. Строка {rightPart.Last().LineNumber}", rightPart.Last().LineNumber);
            }
            return new UpGoElement(rule.Head, 0, rightPart.Last().LineNumber);
        }
        private void SaveStateToLog(Stack<UpGoElement> stack, Stack<UpGoElement> baseStack, RelationEnum rel, Queue<UpGoElement> input)
        {
            LogElement elem = new LogElement(Log.Count + 1, stack, baseStack, rel, input);
            Log.Add(elem);
        }
        public string GetHTML ()
        {
            string start = "<!DOCTYPE html> <html> <head><style> * { margin: 0; padding: 0; } table { margin: 10px auto;  display: block; } td, th { border: 1px solid #333; margin: 2px;font-family: Tahoma, Arial; padding: 3px; text-align: center; } th { background-color: #eaeaea; } .elem { display: inline-block; background: #DDD; height: 25px; padding: 4px; line-height: 25px; border: 1px solid #666; border-radius: 2px; } .rel { border: 1px solid #666; border-radius: 2px; display: inline-block; padding: 4px; } .base { background-color: #8fa; } .rel.greater { background-color: #faa; } .rel.equal { background-color: #35a; } .rel.less { background-color: #3a5; }</style> </head> <body> <table> <tr> <th>#</th> <th>Стек</th> <th>Відн.</th> <th>Вхід</th></tr>";
            StringBuilder body = new StringBuilder();
            for (var i = 0; i < Log.Count; i++)
            {
                body.Append(Log[i].GetHTML());

            }
            string end = "</table></html>";
            return start + body.ToString() + end;

        }
    }
    class LogElement
    {
        int number;
        List<UpGoElement> stack;
        List<UpGoElement> baseStack;
        RelationEnum rel;
        List<UpGoElement> input;
        public LogElement(int number, Stack<UpGoElement> stack, Stack<UpGoElement> baseStack, RelationEnum rel, Queue<UpGoElement> input)
        {
            this.number = number;
            this.stack = new List<UpGoElement>(stack.Count);
            foreach (var e in stack)
            {
                this.stack.Add(e);
            }
            this.stack = this.stack.Take(3).ToList<UpGoElement>();
            this.stack.Reverse();
            
            this.baseStack = new List<UpGoElement>(baseStack.Count);
            foreach (var e in baseStack)
            {
                this.baseStack.Add(e);
            }
            this.rel = rel;
            this.input = new List<UpGoElement>(input.Count);
            foreach (var e in input)
            {
                this.input.Add(e);
            }
            this.input = this.input.Take(5).ToList<UpGoElement>();
        }
        public string GetHTML()
        {
            StringBuilder res = new StringBuilder();
            res.Append("<tr>");
            res.Append($"<td>{this.number}</td>");
            res.Append("<td>");
            foreach (var e in stack)
            {
                res.Append($"<span class=\"elem\">{e.Value}</span>");
            }
            foreach (var e in baseStack)
            {
                res.Append($"<span class=\"elem base\">{e.Value}</span>");
            }
            res.Append("</td>");
            switch (rel)
            {
                case RelationEnum.Greater:
                    res.Append("<td><div class=\"rel greater\">&gt;</div></td>");
                    break;
                case RelationEnum.Equal:
                    res.Append("<td><div class=\"rel equal\">=</div></td>");
                    break;
                case RelationEnum.Less:
                    res.Append("<td><div class=\"rel less\">&lt;</div></td>");
                    break;
                case RelationEnum.Empty:
                    res.Append("<td><div class=\"rel\">Нема відношення</div></td>");
                    break;
                default:
                    break;
            }
            res.Append("<td>");
            foreach (var e in input)
            {
                res.Append($"<span class=\"elem\">{e.Value}</span>");
            }
            res.Append("</td>");

            res.Append("</tr>");
            
            return res.ToString();
        }
        
    }

    class UpGoElement
    {
        public string Value { get; private set; }
        public int LineNumber { get; private set; }
        public int Code { get; private set; }
        public UpGoElement(string value, int code, int lineNumber)
        {
            Value = value;
            LineNumber = lineNumber;
            Code = code;
        }
        public static RelationEnum GetRelation(UpGoElement first, UpGoElement second, List<Relation> relations)
        {
            if (first.Code == -1 && second.Code != -1)
            {
                return RelationEnum.Less;
            }
            else if (first.Code != -1 && second.Code == -1)
            {
                return RelationEnum.Greater;
            }
            else if (first.Code == -1 && second.Code == -1) 
            {
                return RelationEnum.Empty;
            }
            var curRelations = relations.FindAll(elem => CompareValues(first, elem.First)
                                                      && CompareValues(second, elem.Second));

            if (curRelations.Count > 1)
            {
                throw new Exception($"Ошибка: несколько отношений между символами \"{first.Value}\" и \"{second.Value}\".");
            } else if (curRelations.Count == 0)
            {
                throw new LexicalError($"Ошибка: нет отношений между символами \"{first.Value}\" и \"{second.Value}\". Строка {first.LineNumber}", first.LineNumber);
            }
            else
            {
                return curRelations[0].State;
            }
        }
        public static bool CompareValues(UpGoElement one, string value)
        {
            if (value == "ід")
            {
                return one.Code == 31;
            }
            else if (value == "константа")
            {
                return one.Code == 32;
            }
            else if (value == "рядок")
            {
                return one.Code == 33;
            }
            else
            {
                return one.Value == value;
            }
        }
    }
}
