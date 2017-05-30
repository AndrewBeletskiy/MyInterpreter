using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    class Executor
    {
        public Form1 Parent { get; private set; }
        public Poliz Poliz { get; private set; }
        public int CurrentOperation { get; private set; }
        public PolizElement CurrentElem => Poliz[CurrentOperation];
        public Dictionary<string, IDClass> Identifiers { get; private set; }
        public StringTable Strings => Poliz.STRING;
        Stack<object> Stack;
        public bool Runned { get; private set; }


        public Executor(Form1 parent, Poliz poliz)
        {
            Parent = parent;
            parent.Entered += OnEnter;
            Poliz = poliz;
            CurrentOperation = 0;
            Identifiers = new Dictionary<string, IDClass>();
            for (var j = 0; j < poliz.ID.Length; j++)
            {
                Identifiers.Add(poliz.ID[j].Name, new IDClass(poliz.ID[j].Name));
            }
            Stack = new Stack<object>();
        }
        private double GetDoubleFromStack()
        {
            object obj = Stack.Pop();
            var elem = obj as PolizElement;
            if (elem != null)
            {
                if (elem.Code == PolizElementCode.ID)
                {
                    return Identifiers[elem.Substring].Value;
                }
                else 
                {
                    return Poliz.CONST[elem.ClassElementNumber];
                }
            } else
            {
                return (double)obj;
            }
            return -1e8;
        }
        public void OnEnter()
        {
            if (CurrentOperation < Poliz.Length)
                Run();
        }
        private void ReadOperation()
        {
            var idName = (Stack.Peek() as PolizElement).Substring;
            var str = Parent.GetInput();
            if (str == null)
            {
                Runned = false;
            }
            else
            {
                double value = 0;
                if (double.TryParse(str, out value))
                {
                    Identifiers[idName].Value = value;
                    CurrentOperation++;
                    Stack.Pop();
                }
                else
                {
                    Runned = false;
                }
            }
        }
        private void WriteOperation()
        {
            var elem = Stack.Pop() as PolizElement;
            if (elem.Code == PolizElementCode.ID)
            {
                Parent.Write(Identifiers[elem.Substring].Value.ToString());
            } else
            {
                Parent.Write(Poliz.STRING[elem.ClassElementNumber]);
            }
            
        }
        private void OrOperation()
        {
            var first = (bool)Stack.Pop();
            var second = (bool)Stack.Pop();
            Stack.Push(first || second);
        }
        private void AndOperation()
        {
            var first = (bool)Stack.Pop();
            var second = (bool)Stack.Pop();
            Stack.Push(first && second);
        }
        private void NotOperation()
        {
            var boolValue = (bool)Stack.Pop();
            Stack.Push(!boolValue);
        }
        private void PlusOperation()
        {
            var first = GetDoubleFromStack();
            var second = GetDoubleFromStack();
            Stack.Push(first + second);
        }
        private void MinusOperation()
        {
            var first = GetDoubleFromStack();
            var second = GetDoubleFromStack();
            Stack.Push(second - first);
        }
        private void MultiplyOperation()
        {
            var first = GetDoubleFromStack();
            var second = GetDoubleFromStack();
            Stack.Push(second * first);
        }
        private void DivideOperation()
        {
            var first = GetDoubleFromStack();
            var second = GetDoubleFromStack();
            if (first != 0)
            {
                Stack.Push(second / first);
            }
            else
            {
                CurrentOperation = Poliz.Length;
                Parent.Write("DIVIDING BY ZERO EXCEPTION");
                Runned = false;
            }           
        }
        private void AssignmentOperation()
        {
            var value = GetDoubleFromStack();
            var idName = (Stack.Pop() as PolizElement).Substring;
            Identifiers[idName].Value = value;
        }
        private void CompareOperation(Func<double,double,bool> func)
        {
            var first = GetDoubleFromStack();
            var second = GetDoubleFromStack();
            bool res = func(second, first);
            Stack.Push(res);
        }
        private void BPOperation()
        {
            var label = Stack.Pop() as Label;
            Stack.Clear();
            CurrentOperation = label.Position;
        }
        private void UPLOperation()
        {
            var label = Stack.Pop() as Label;
            var condition = (bool)Stack.Pop();
            if (!condition)
            {
                Stack.Clear();
                CurrentOperation = label.Position;
            } else
            {
                CurrentOperation++;
            }
        }
        private void LabelOperation()
        {
            var label = CurrentElem as Label;
            if (label.Position != CurrentOperation + 1)
            {
                Stack.Push(label);
            }
        }

        public void Run()
        {
            Runned = true;
            while (CurrentOperation < Poliz.Length && Runned)
            {
                if (CurrentElem.IsOperand)
                {
                    Stack.Push(CurrentElem);
                    CurrentOperation++;
                    continue;
                }
                switch (CurrentElem.Code)
                {
                    case PolizElementCode.READ:
                        ReadOperation();
                        break;
                    case PolizElementCode.WRITE:
                        WriteOperation();
                        CurrentOperation++;
                        break;
                    case PolizElementCode.OR:
                        OrOperation();
                        CurrentOperation++;
                        break;
                    case PolizElementCode.NOT:
                        NotOperation();
                        CurrentOperation++;
                        break;

                    case PolizElementCode.AND:
                        AndOperation();
                        CurrentOperation++;
                        break;
                    case PolizElementCode.PLUS:
                        PlusOperation();
                        CurrentOperation++;
                        break;
                    case PolizElementCode.MINUS:
                        MinusOperation();
                        CurrentOperation++;
                        break;
                    case PolizElementCode.MULTIPLY:
                        MultiplyOperation();
                        CurrentOperation++;
                        break;
                    case PolizElementCode.DIVIDE:
                        DivideOperation();
                        CurrentOperation++;
                        break;
                    case PolizElementCode.ASSIGNMENT:
                        AssignmentOperation();
                        CurrentOperation++;
                        break;
                    case PolizElementCode.EQUAL:
                        CompareOperation((a, b) => a == b);
                        CurrentOperation++;
                        break;
                    case PolizElementCode.MORE:
                        CompareOperation((a, b) => a > b);
                        CurrentOperation++;
                        break;
                    case PolizElementCode.LESS:
                        CompareOperation((a, b) => a < b);
                        CurrentOperation++;
                        break;
                    case PolizElementCode.MORE_OR_EQUAL:
                        CompareOperation((a, b) => a >= b);
                        CurrentOperation++;
                        break;
                    case PolizElementCode.LESS_OR_EQUAL:
                        CompareOperation((a, b) => a <= b);
                        CurrentOperation++;
                        break;
                    case PolizElementCode.NOT_EQUAL:
                        CompareOperation((a, b) => a != b);
                        CurrentOperation++;
                        break;
                    case PolizElementCode.BP:
                        BPOperation();
                        break;
                    case PolizElementCode.UPL:
                        UPLOperation();
                        break;
                    case PolizElementCode.LABEL:
                        LabelOperation();
                        CurrentOperation++;
                        break;
                }
            }
            if (CurrentOperation == Poliz.Length)
            {
                Parent.Write("\n---- PROGRAM ENDED ----");
            }
            Runned = false;
        }
    }

    public class IDClass
    {
        public string Name { get; private set; }
        public double Value { get; set; }
        public IDClass(string name)
        {
            Name = name;
            Value = 0;
        }
        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}
