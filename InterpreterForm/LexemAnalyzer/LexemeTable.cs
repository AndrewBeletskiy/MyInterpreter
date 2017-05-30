using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    class LexemeTable
    {
        List<Lexeme> list_;
        List<char> emptyCharacters_;
        public int LastKeywordIndex { get; set; }
        public LexemeTable(int capacity = 8)
        {
            list_ = new List<Lexeme>(capacity);
            emptyCharacters_ = new List<char>();
        }
        public void Add(Lexeme lexeme)
        {
            list_.Add(lexeme);
        }
        public void Add(string lexeme, bool isDelimiter = false)
        {
            Add(lexeme, list_.Count + 1, isDelimiter);
        }
        public void Add(string lexeme, int code, bool isDelimiter = false)
        {
            Lexeme newLexeme = new Lexeme(lexeme, code, isDelimiter);
            Add(newLexeme);
        }
        public int IndexOf(string lexeme)
        {
            Lexeme founded = GetByTextValue(lexeme);
            if (founded == null)
                return -1;
            else
                return list_.IndexOf(founded);
        }
        public int CodeOf(string value)
        {
            Lexeme founded = GetByTextValue(value);
            return (founded != null) ? founded.Code : 0;
        }
        public Lexeme GetByTextValue(string value)
        {
            foreach (Lexeme lexeme in list_)
            {
                if (lexeme.TextValue == value)
                {
                    return lexeme;
                }
            }
            return null;
        }
        public void AddEmptyCharacter(char delimiter)
        {
            emptyCharacters_.Add(delimiter);
        }
        public bool IsEmptyCharacter(char character)
        {
            return emptyCharacters_.Contains(character);
        }
        public int GetKeywordCode(string word)
        {
            var index = IndexOf(word);
            return (index <= LastKeywordIndex && index >= 0) ?  GetByTextValue(word).Code : 0;
        }
        public void ShowConsole()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("LEXEME TABLE");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Lexeme\t| Code\t| Delimiter|");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            for (var i = 0; i < list_.Count; i++)
            {
                Console.WriteLine(String.Format("{0}\t| {1}\t| {2}\t   |",
                    list_[i].TextValue,
                    list_[i].Code,
                    list_[i].IsDelimiter ? "Yes" : ""));
            }
        }
        public object GetDataGridList()
        {
            return (from lexem in list_
                    select new { Lexem = lexem.TextValue, Code = lexem.Code }).ToList();
        }
    }
}
