using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    class Label : PolizElement
    {

        public int Number { get; private set; }
        public int Position { get; set; }
        public Label(int number, int position)
            :base(new LexemeAnalyzerOutTableElement(0,$"m{number}",(int)PolizElementCode.LABEL, 0))
        {
            this.Number = number;
            this.Position = position;
            IsOperand = false;
            Priority = -1;
        }
        public Label(int number) : this(number, 0) { }
        public override string GetHTML(int number)
        {
            if (number == Position -1)
            {
                return $"<span class=\'elem base\'> {Substring}:</span>";
            } else
            {
                return $"<span class=\'elem\'> {Substring}</span>";
            }
            
        }
        public override string ToString()
        {
            return $"{Substring}";
        }
    }
    class DynamicOperator : PolizElement
    {
        public Label First;
        public Label Last;
        public DynamicOperator(string elementString, int lexemCode)
            :base(new LexemeAnalyzerOutTableElement(0, elementString,lexemCode, 0))
        {}
        public static DynamicOperator GenerateIf()
        {
            var res = new DynamicOperator("if", (int)PolizElementCode.IF);
            res.Priority = 0;
            return res;
        }
        public static DynamicOperator GenerateDo()
        {
            var res = new DynamicOperator("do", (int)PolizElementCode.DO);
            res.Priority = 0;
            return res;
        }
        public override string ToString()
        {
            var res = new StringBuilder();
            res.Append(Substring).Append(" ");
            res.Append(First?.ToString() ?? "None");
            if (Last != null)
                res.Append(" " + Last.ToString());
            else
                res.Append(" None");

            return res.ToString();
        }
    }

    class Poliz
    {
        public IdentifierTable ID;
        public StringTable STRING;
        public ConstantsTable CONST;
        public List<Label> labels;
        List<PolizElement> INPUT;
        List<PolizElement> OUTPUT;
        public int Length => OUTPUT.Count;
        public PolizElement this[int index]
        {
            get
            {
                return OUTPUT[index];
            }
            set
            {
                OUTPUT[index] = value;
            }
        }
        public Poliz(IdentifierTable idTable, StringTable stringTable, ConstantsTable constTable, LexemeAnalyzerOutTable input)
        {
            ID = idTable;
            STRING = stringTable;
            CONST = constTable;
            INPUT = new List<PolizElement>(input.Length);
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i].LexemeCode == (int)LexemeCodes.DO)
                {
                    INPUT.Add(DynamicOperator.GenerateDo());
                } else if (input[i].LexemeCode == (int)LexemeCodes.IF)
                {
                    INPUT.Add(DynamicOperator.GenerateIf());
                } else {
                    INPUT.Add(new PolizElement(input[i]));
                }
                
            }
            Generate();
        }
        private void Generate()
        {
            OUTPUT = new List<PolizElement>();
            labels = new List<Label>();
            Stack<PolizElement> stack = new Stack<PolizElement>();
            int i = 1;
            while (i < INPUT.Count-1)
            {
                var current = INPUT[i];
                
                if (current.Code == PolizElementCode.WRITE || current.Code == PolizElementCode.READ)
                {
                    i += 2;
                    while (INPUT[i].Code == PolizElementCode.ID || INPUT[i].Code == PolizElementCode.STRING)
                    {
                        OUTPUT.Add(INPUT[i]);
                        OUTPUT.Add(current);
                        i++;
                        if (INPUT[i].Code == PolizElementCode.CLOSE_PARANTHESIS)
                        {
                            i++;
                            break;
                        } else
                        {
                            i++;
                        }
                    }
                    if (INPUT[i].Code == PolizElementCode.SEMICOLON)
                    {
                        if (stack.Count == 0 || stack.Peek().Code != PolizElementCode.IF)
                        {
                            i++;
                        }
                    }
                        
                    continue;
                }

                if (current.IsOperand)
                {
                    OUTPUT.Add(current);
                    i++;
                    continue; // NEXT ELEMENT OF INPUT
                }
                if (current.Priority == -1)
                {
                    i++;
                    continue;
                }
                if (stack.Count == 0)
                {
                    if (current.Code == PolizElementCode.DO)
                        makeDoAddition(current);
                    if (current.Code != PolizElementCode.SEMICOLON)
                        stack.Push(current);
                    
                        
                    i++;
                    continue; // NEXT ELEMENT OF INPUT
                }
                var stackElement = stack.Peek();
                if (current.Code == PolizElementCode.CLOSE_BRACKET)
                {
                    StackToElemCodeToOutput(stack, PolizElementCode.OPEN_BRACKET);
                    i++;
                    continue; // NEXT ELEMENT OF INPUT
                }
                else if (current.Code == PolizElementCode.CLOSE_PARANTHESIS)
                {
                    StackToElemCodeToOutput(stack, PolizElementCode.OPEN_PARENTHESIS);
                    i++;
                    continue; // NEXT ELEMENT OF INPUT
                }
                if (current.Priority == 0)
                {
                    if (current.Code == PolizElementCode.DO)
                        makeDoAddition(current);
                    stack.Push(current);
                    i++;
                    continue;
                }
                if (current.Code == PolizElementCode.SEMICOLON && stackElement.Code == PolizElementCode.END)
                {
                    StackToElemCodeToOutput(stack, PolizElementCode.DO);
                    //i++;
                    continue;
                }
                if (current.Priority <= stackElement.Priority)
                {
                    StackToOutput(stack);
                    continue; // CHECK NEXT STACK ELEMENT
                }
                if (current.Code == PolizElementCode.THEN)
                {
                    var m1 = GenerateLabel();
                    var ifElem = stackElement as DynamicOperator;
                    ifElem.First = m1;
                    OUTPUT.Add(m1);
                    OUTPUT.Add(PolizElement.GenerateUPL());
                    i++;
                    continue; // TO NEXT ELEMENT
                }
                if (current.Code == PolizElementCode.ELSE)
                {
                    var m2 = GenerateLabel();
                    var ifElem = stackElement as DynamicOperator;
                    ifElem.Last = m2;
                    OUTPUT.Add(m2);
                    OUTPUT.Add(PolizElement.GenerateBP());
                    OUTPUT.Add(ifElem.First);
                    ifElem.First.Position = OUTPUT.Count;
                    i++;
                    continue; // TO NEXT ELEMENT
                }
                if (current.Code == PolizElementCode.DO)
                {
                    DynamicOperator doElem = makeDoAddition(current);
                    stack.Push(doElem);
                    i++;
                    continue;
                }
                if (current.Code == PolizElementCode.SEMICOLON && stackElement.Code == PolizElementCode.DO)
                {
                    var doElem = stackElement as DynamicOperator;
                    if (doElem.Last == null) { 
                        var m2 = GenerateLabel();
                        doElem.Last = m2;
                        OUTPUT.Add(m2);
                        OUTPUT.Add(PolizElement.GenerateUPL());
                    }
                    i++;
                    continue;
                }
                if (current.Code == PolizElementCode.END)
                {
                    if (stackElement.Code != PolizElementCode.DO)
                    {
                        StackToOutput(stack);
                        continue;
                    }
                    var doElem = stackElement as DynamicOperator;

                    OUTPUT.Add(doElem.First);
                    OUTPUT.Add(PolizElement.GenerateBP());
                    OUTPUT.Add(doElem.Last);
                    doElem.Last.Position = OUTPUT.Count;
                    stack.Push(current);
                    i++;
                    continue;
                }
                
                if (current.Code == PolizElementCode.SEMICOLON)
                {
                    while (stackElement.Code == PolizElementCode.IF)
                    {
                        var ifElem = stackElement as DynamicOperator;
                        OUTPUT.Add(ifElem.Last);
                        ifElem.Last.Position = OUTPUT.Count;
                        stack.Pop();
                        if (stack.Count > 0)
                            stackElement = stack.Peek();
                        else break;
                    }
                    i++;
                    continue;
                }
                i++;
                stack.Push(current);

            }
            while (stack.Count > 0)
            {
                StackToOutput(stack);
            }
        }

        private DynamicOperator makeDoAddition(PolizElement current)
        {
            var doElem = current as DynamicOperator;
            var m1 = GenerateLabel();

            OUTPUT.Add(m1);
            m1.Position = OUTPUT.Count;
            doElem.First = m1;

            return doElem;
        }

        private void StackToElemCodeToOutput(Stack<PolizElement> stack, PolizElementCode code)
        {
            while (stack.Peek().Code != code)
                StackToOutput(stack);
            StackToOutput(stack);
        }
        private void StackToOutput(Stack<PolizElement> stack)
        {
            var elem = stack.Pop();
            if (elem.Code != PolizElementCode.IF && elem.Code != PolizElementCode.THEN 
                && elem.Code != PolizElementCode.ELSE && elem.Code != PolizElementCode.OPEN_BRACKET
                && elem.Code != PolizElementCode.OPEN_PARENTHESIS && elem.Code != PolizElementCode.SEMICOLON 
                && elem.Code != PolizElementCode.END && elem.Code != PolizElementCode.DO)
            {
                OUTPUT.Add(elem);
            }
        }
        
        private Label GenerateLabel()
        {
            Label label = new Label(labels.Count);
            labels.Add(label);
            return label;
        }
        public string GetHTML()
        {
            StringBuilder res = new StringBuilder();

            string start = "<!DOCTYPE html> <html> <head><style> * { margin: 0; padding: 0; } table { margin: 10px auto;  display: block; } td, th { border: 1px solid #333; margin: 2px;font-family: Tahoma, Arial; padding: 3px; text-align: center; } th { background-color: #eaeaea; } .elem { display: inline-block; background: #DDD; height: 25px; padding: 4px; line-height: 25px; border: 1px solid #666; border-radius: 2px; margin-left:  2px;} .rel { border: 1px solid #666; border-radius: 2px; display: inline-block; padding: 4px; } .base { background-color: #8fa; } .rel.greater { background-color: #faa; } .rel.equal { background-color: #35a; } .rel.less { background-color: #3a5; } h1 { font-family: Tahoma; } </style> </head> <body> <h1>Полиз</h1>";
            
            for (var i = 0; i < OUTPUT.Count; i++)
            {
                res.Append(OUTPUT[i].GetHTML(i));
            }
            string end = "</body></html>";
            return start + res.ToString() + end;

        }
    }

    public enum PolizElementCode
    {
        READ = 1,
        WRITE,
        IF,
        THEN,
        ELSE,
        DO,
        WHILE,
        END,
        OR,
        NOT,
        AND,
        OPEN_CURLY_BRACKET,
        CLOSE_CURLY_BRACKET,
        OPEN_PARENTHESIS,
        CLOSE_PARANTHESIS,
        OPEN_BRACKET,
        CLOSE_BRACKET,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        SEMICOLON,
        COMMA,
        ASSIGNMENT,
        EQUAL,
        MORE,
        LESS,
        MORE_OR_EQUAL,
        LESS_OR_EQUAL,
        NOT_EQUAL,
        ID,
        CONST,
        STRING,
        BP,
        UPL,
        LABEL,
        BOOL
    }
    public class PolizElement : LexemeAnalyzerOutTableElement
    {
        public bool IsOperand { get; protected set; }
        public PolizElementCode Code;
        public int Priority { get; set; }

        public PolizElement(LexemeAnalyzerOutTableElement elem)
            : base(elem.StringNumber, elem.Substring, elem.LexemeCode, elem.ClassElementNumber)
        {
            Code = (PolizElementCode)elem.LexemeCode;
            switch (Code)
            {
                case PolizElementCode.ID:
                case PolizElementCode.CONST:
                case PolizElementCode.STRING:
                    IsOperand = true;
                    break;
                default:
                    IsOperand = false;
                    break;
            }
            switch (Code)
            {
                case PolizElementCode.OPEN_PARENTHESIS:
                case PolizElementCode.IF:
                case PolizElementCode.DO:
                case PolizElementCode.OPEN_BRACKET:
                case PolizElementCode.READ:
                case PolizElementCode.WRITE:
                    Priority = 0;
                    break;
                case PolizElementCode.CLOSE_PARANTHESIS:
                case PolizElementCode.THEN:
                case PolizElementCode.ELSE:
                case PolizElementCode.CLOSE_BRACKET:
                case PolizElementCode.END:
                case PolizElementCode.SEMICOLON:
                    Priority = 1;
                    break;
                case PolizElementCode.ASSIGNMENT:
                    Priority = 2;
                    break;
                case PolizElementCode.OR:
                    Priority = 3;
                    break;
                case PolizElementCode.AND:
                    Priority = 4;
                    break;
                case PolizElementCode.NOT:
                    Priority = 5;
                    break;
                case PolizElementCode.MORE:
                case PolizElementCode.MORE_OR_EQUAL:
                case PolizElementCode.NOT_EQUAL:
                case PolizElementCode.LESS:
                case PolizElementCode.LESS_OR_EQUAL:
                case PolizElementCode.EQUAL:
                    Priority = 6;
                    break;
                case PolizElementCode.PLUS:
                case PolizElementCode.MINUS:
                    Priority = 7;
                    break;
                case PolizElementCode.MULTIPLY:
                case PolizElementCode.DIVIDE:
                    Priority = 8;
                    break;
                default:
                    Priority = -1;
                    break;
            }
        }

        public virtual string GetHTML(int number)
        {
            if (!IsOperand)
            {
                return $"<span class=\'elem\'> {Substring}</span>";
            } else
            {
                return $"<span class=\'rel\'> {Substring}</span>";
            }

        }

        public static PolizElement GenerateUPL()
        {
            var res = new PolizElement(new LexemeAnalyzerOutTableElement(0, "UPL", (int)PolizElementCode.UPL, 0));
            return res;
        }
        public static PolizElement GenerateBP()
        {
            var res = new PolizElement(new LexemeAnalyzerOutTableElement(0, "BP", (int)PolizElementCode.BP, 0));
            return res;
        }
        public static PolizElement GenerateTrue()
        {
            var res = new PolizElement(new LexemeAnalyzerOutTableElement(0, "true", (int)PolizElementCode.BOOL, 1));
            return res;
        }
        public static PolizElement GenerateFalse()
        {
            var res = new PolizElement(new LexemeAnalyzerOutTableElement(0, "false", (int)PolizElementCode.BOOL, 0));
            return res;
        }
        

        public override string ToString()
        {
            return Substring;
        }
    }
}
