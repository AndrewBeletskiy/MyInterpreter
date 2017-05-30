using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    enum CharacterClass
    {
        Letter, Digit, Delimiter, Colon, Equal, More, Less, ExclamationPoint, Point, Quote, Empty, Error
    }
    static class CharacterClassExtension
    {
        public static CharacterClass GetClass(this char character)
        {
            if (character == ' ' || character == '\n' || character == '\r' || character == '\t')
            { 
                return CharacterClass.Empty;
            }
            else if ((character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z'))
            {
                return CharacterClass.Letter;
            }
            else if (char.IsDigit(character))
            {
                return CharacterClass.Digit;
            }
            switch (character)
            {
                case '{':
                case '}':
                case '(':
                case ')':
                case '[':
                case ']':
                case '+':
                case '-':
                case '*':
                case '/':
                case ';':
                case ',':
                case '\\':
                    return CharacterClass.Delimiter;
                case ':':
                    return CharacterClass.Colon;
                case '=':
                    return CharacterClass.Equal;
                case '>':
                    return CharacterClass.More;
                case '<':
                    return CharacterClass.Less;
                case '!':
                    return CharacterClass.ExclamationPoint;
                case '"':
                    return CharacterClass.Quote;
                case '.':
                    return CharacterClass.Point;
                default:
                    return CharacterClass.Error;
            }
        }
    }
}
