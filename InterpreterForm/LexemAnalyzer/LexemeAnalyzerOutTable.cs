using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    public class LexemeAnalyzerOutTableElement
    {
        public int StringNumber { get; private set; }
        public string Substring { get; private set; }
        public int LexemeCode { get; private set; }
        public int ClassElementNumber { get; private set; }
        public LexemeAnalyzerOutTableElement(int stringNumber, string substring, int lexemeCode, int classElementNumber)
        {
            StringNumber = stringNumber;
            Substring = substring;
            LexemeCode = lexemeCode;
            ClassElementNumber = classElementNumber;
        }

    }
    public class LexemeAnalyzerOutTable
    {
        List<LexemeAnalyzerOutTableElement> table_;
        public LexemeAnalyzerOutTableElement this[int index]
        {
            get { return table_[index]; }
        }
        public int Length
        {
            get { return table_.Count; }
        }
        public LexemeAnalyzerOutTable()
        {
            table_ = new List<LexemeAnalyzerOutTableElement>();
        }
        public void Add(int stringNumber, Lexeme lexeme, int classElementNumber = 0)
        {
            table_.Add(new LexemeAnalyzerOutTableElement(stringNumber, lexeme.TextValue, lexeme.Code, classElementNumber));
        }
        public void Add(int stringNumber, string substring, int lexemeCode, int classElementNumber = 0)
        {
            table_.Add(new LexemeAnalyzerOutTableElement(stringNumber, substring, lexemeCode, classElementNumber));
        }
        public void ShowConsole()
        {
            ColoredTable table = new InterpreterForm.ColoredTable(table_.Count + 1, 4, " | ");
            table.AddString("#", 0, 0, ConsoleColor.White, ConsoleColor.Red);
            table.AddString("Substring", 0, 1, ConsoleColor.White, ConsoleColor.Red);
            table.AddString("Lexem code or class", 0, 2, ConsoleColor.White, ConsoleColor.Red);
            table.AddString("Number in class", 0, 3, ConsoleColor.White, ConsoleColor.Red);
            for (var i = 0; i < table_.Count; i++)
            {
                var item = table_[i];
                table.AddString(item.StringNumber.ToString(), i + 1, 0);
                table.AddString(item.Substring, i + 1, 1);
                table.AddString(item.LexemeCode.ToString(), i + 1, 2);
                table.AddString((item.ClassElementNumber > 0) ? item.ClassElementNumber.ToString() : "", i + 1, 3);
            }
            table.Normalize();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("OUT TABLE OF LEXEM: ");
            Console.ForegroundColor = ConsoleColor.White;
            table.Show();
        }
        public object GetDataGridList()
        {
            return (from lexem in table_
                    select new { StringNumber = lexem.StringNumber,
                                 Substring = lexem.Substring,
                                 Code = lexem.LexemeCode,
                                 ClassIndex = (lexem.ClassElementNumber > 0) ? lexem.ClassElementNumber.ToString() : ""
                               }).ToList();
        }
    }
}
