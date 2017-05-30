using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    public class Lexeme
    {
        public string TextValue { get; private set; }
        public int Code { get; private set; }
        public bool IsDelimiter { get; private set; }
        public Lexeme(string textValue, int code, bool isDelimiter = false)
        {
            TextValue = textValue;
            Code = code;
            IsDelimiter = isDelimiter;
        }
    }
}
