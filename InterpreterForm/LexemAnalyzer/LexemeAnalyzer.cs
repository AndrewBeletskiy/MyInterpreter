#define TEST1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace InterpreterForm
{
    public enum LexemeCodes{
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
        STRING
    }
    class Automat
    {
        private int state_;
        public string CurrentString { get; private set; }
        private int stringNumber_;
        private LexemeAnalyzerOutTable table_;
        private LexemeTable lexemeTable_;
        private IdentifierTable identifierTable_;
        private ConstantsTable constantsTable_;
        private StringTable stringTable_;
        public bool IsDone
        {
            get { return state_ == 1; }
        }
        public Automat(LexemeTable lexemeTable, LexemeAnalyzerOutTable table, IdentifierTable identifierTable,
            ConstantsTable constantsTable, StringTable stringTable)
        {
            state_ = 1;
            CurrentString = "";
            table_ = table;
            lexemeTable_ = lexemeTable;
            identifierTable_ = identifierTable;
            constantsTable_ = constantsTable;
            stringTable_ = stringTable;
            stringNumber_ = 1;
        }

        public void PushChar(char c)
        {

            if (c == '\n')
                stringNumber_++;

            CharacterClass charClass = c.GetClass();
            if (charClass == CharacterClass.Error)
                throw new LexicalError(String.Format("Illegal character {0} in string \"{1}\" on {2} line", c, CurrentString, stringNumber_), stringNumber_);
            switch (state_)
            {
                case 1:
                    CurrentString += c;
                    switch (charClass)
                    {
                        case CharacterClass.Letter:
                            state_ = 2;
                            break;
                        case CharacterClass.Digit:
                            state_ = 3;
                            break;
                        case CharacterClass.Point:
                            state_ = 4;
                            break;
                        case CharacterClass.Quote:
                            state_ = 9;
                            break;
                        case CharacterClass.Delimiter:
                            table_.Add(stringNumber_, lexemeTable_.GetByTextValue(CurrentString), 0);
                            ToDefault();
                            break;
                        case CharacterClass.Colon:
                            state_ = 10;
                            break;
                        case CharacterClass.Equal:
                            table_.Add(stringNumber_, lexemeTable_.GetByTextValue(CurrentString), 0);
                            ToDefault();
                            break;
                        case CharacterClass.More:
                            state_ = 11;
                            break;
                        case CharacterClass.Less:
                            state_ = 12;
                            break;
                        case CharacterClass.ExclamationPoint:
                            state_ = 13;
                            break;
                        case CharacterClass.Empty:
                            ToDefault();
                            break;
                    }
                    break;
                case 2:

                    switch (charClass)
                    {
                        case CharacterClass.Letter:
                        case CharacterClass.Digit:
                            CurrentString += c;
                            break;
                        default:
                            if (lexemeTable_.GetKeywordCode(CurrentString) == 0)
                            {
                                int code = identifierTable_.GetCodeOf(CurrentString);
                                if (code == 0)
                                {
                                    code = identifierTable_.Add(CurrentString);
                                }
                                table_.Add(stringNumber_, CurrentString, (int)LexemeCodes.ID, code);
                            } else
                            {
                                table_.Add(stringNumber_, lexemeTable_.GetByTextValue(CurrentString));
                            }
                            ToDefault();
                            PushChar(c);
                            break;
                    }
                    break;
                case 3:
                    if (charClass == CharacterClass.Digit)
                    {
                        CurrentString += c;
                        state_ = 3;
                    }
                    else if (c == 'E' || c == 'e')
                    {
                        CurrentString += c;
                        state_ = 6;
                    }
                    else if (charClass == CharacterClass.Point)
                    {
                        CurrentString += c;
                        state_ = 5;
                    }
                    else
                    {
                        double constValue = GetDoubleFromString(CurrentString);
                        table_.Add(stringNumber_, CurrentString, (int)LexemeCodes.CONST, constantsTable_.Add(constValue));
                        ToDefault();
                        PushChar(c);
                    }
                    break;
                case 4:
                    if (charClass == CharacterClass.Digit)
                    {
                        state_ = 5;
                        CurrentString += c;
                    } else
                    {
                        throw new LexicalError(String.Format("Error in line {0}: Must be digit after a point in a number constant.", stringNumber_), stringNumber_);
                    }
                    break;
                case 5:
                    if (charClass == CharacterClass.Digit)
                    {
                        state_ = 5;
                        CurrentString += c;
                    }
                    else if (c == 'E' || c == 'e')
                    {
                        state_ = 6;
                        CurrentString += c;
                    }
                    else
                    {
                        double constValue = GetDoubleFromString(CurrentString);
                        table_.Add(stringNumber_, CurrentString, 32, constantsTable_.Add(constValue));
                        ToDefault();
                        PushChar(c);
                    }
                    break;
                case 6:
                    if (c == '+' || c == '-')
                    {
                        CurrentString += c;
                        state_ = 7;
                    } 
                    else if (charClass == CharacterClass.Digit)
                    {
                        CurrentString += c;
                        state_ = 8;
                    }
                    else
                    {
                        throw new LexicalError(String.Format("Error in line {0}: Must be a number or a sign after E in a number constant.", stringNumber_), stringNumber_);
                    }
                    break;
                case 7:
                    if (charClass == CharacterClass.Digit)
                    {
                        CurrentString += c;
                        state_ = 8;
                    }
                    else
                    {
                        throw new LexicalError(String.Format("Error in line {0}: Must be number after the sign in a number constant.", stringNumber_), stringNumber_);
                    }
                    break;
                case 8:
                    if (charClass == CharacterClass.Digit)
                    {
                        state_ = 8;
                        CurrentString += c;
                    }
                    else
                    {
                        double constValue = GetDoubleFromString(CurrentString);
                        table_.Add(stringNumber_, CurrentString, (int)LexemeCodes.CONST, constantsTable_.Add(constValue));
                        ToDefault();
                        PushChar(c);
                    }
                    break;
                case 9:
                    if (charClass == CharacterClass.Letter || charClass == CharacterClass.Digit
                        || c == '.' || c == ',' || c == '=' || c == ';' || c == ':' || c == ' ' || c == '\\')
                    {
                        CurrentString += c;
                        state_ = 9;
                    } else if (c == '"')
                    {
                        CurrentString += c;
                        string stringValue = CurrentString.Substring(1, CurrentString.Length - 2);
                        while (stringValue.Contains("\\n"))
                        {
                            stringValue = stringValue.Replace("\\n", "\n");
                        }
                        while (stringValue.Contains("\\t"))
                        {
                            stringValue = stringValue.Replace("\\t", "\t");
                        }
                        table_.Add(stringNumber_, CurrentString, (int)LexemeCodes.STRING, stringTable_.Add(stringValue));
                        ToDefault();
                    }
                    else 
                    {
                        throw new LexicalError(String.Format("Error in line {0}: Illegal string constants. String constants must contain only digits, letters or characters: '.' ',' ';'  ':' '='.", stringNumber_), stringNumber_);
                    }
                    break;
                case 10:
                    if (c == '=')
                    {
                        CurrentString += c;
                        table_.Add(stringNumber_, lexemeTable_.GetByTextValue(CurrentString));
                        ToDefault();
                    }
                    else
                    {
                        throw new LexicalError(String.Format("Error in line {0}: Illegal operator \":\"", stringNumber_), stringNumber_);
                    }
                    break;
                case 11:
                    if (c == '=')
                    {
                        table_.Add(stringNumber_, lexemeTable_.GetByTextValue(">="));
                        ToDefault();
                    }
                    else
                    {
                        table_.Add(stringNumber_, lexemeTable_.GetByTextValue(">"));
                        ToDefault();
                        PushChar(c);
                    }
                    break;
                case 12:
                    if (c == '=')
                    {
                        table_.Add(stringNumber_, lexemeTable_.GetByTextValue("<="));
                        ToDefault();
                    }
                    else
                    {
                        table_.Add(stringNumber_, lexemeTable_.GetByTextValue("<"));
                        ToDefault();
                        PushChar(c);
                    }
                    break;
                case 13:
                    if (c == '=')
                    {
                        table_.Add(stringNumber_, lexemeTable_.GetByTextValue("!="));
                        ToDefault();
                    }
                    else
                    {
                        throw new LexicalError(String.Format("Error in line {0}: Illegal operator \"!\"", stringNumber_), stringNumber_);
                    }
                    break;
                default:
                    throw new Exception(String.Format("State must can not be {0}", state_));
            }
        }
        private double GetDoubleFromString(string doubleString)
        {
            if (doubleString[doubleString.Length - 1] == '.')
            {
                doubleString += '0';
            }
            return double.Parse(doubleString.Replace('.', ','));
        }
        private void ToDefault()
        {
            state_ = 1;
            CurrentString = "";
        }
        public void PushString(string s)
        {
            for (var i = 0; i < s.Length; i++)
            {
                PushChar(s[i]);
            }
        }

    }
    class LexemeAnalyzer
    {
        public string ErrorMessage { get; private set; }
        public bool IsError { get; private set; }
        public int ErrorLine { get; private set; }
        public static LexemeTable LexemTable = new LexemeTable();
        public IdentifierTable IdentifierTable { get; private set; }
        public ConstantsTable ConstantsTable { get; private set; }
        public StringTable StringTable { get; private set; }
        public LexemeAnalyzerOutTable OutTable { get; private set; }
        static LexemeAnalyzer()
        {
            Initialize();
        }
        static void Initialize()
        {
            LexemTable = new LexemeTable(32);
            LexemTable.Add("read", (int)LexemeCodes.READ);
            LexemTable.Add("write", (int)LexemeCodes.WRITE);
            LexemTable.Add("if", (int)LexemeCodes.IF);
            LexemTable.Add("then", (int)LexemeCodes.THEN);
            LexemTable.Add("else", (int)LexemeCodes.ELSE);
            LexemTable.Add("do", (int)LexemeCodes.DO);
            LexemTable.Add("while", (int)LexemeCodes.WHILE);
            LexemTable.Add("end", (int)LexemeCodes.END);
            LexemTable.Add("or", (int)LexemeCodes.OR);
            LexemTable.Add("not", (int)LexemeCodes.NOT);
            LexemTable.Add("and", (int)LexemeCodes.AND);
            LexemTable.Add("{", (int)LexemeCodes.OPEN_CURLY_BRACKET, true);
            LexemTable.Add("}", (int)LexemeCodes.CLOSE_CURLY_BRACKET, true);
            LexemTable.Add("(", (int)LexemeCodes.OPEN_PARENTHESIS, true);
            LexemTable.Add(")", (int)LexemeCodes.CLOSE_PARANTHESIS, true);
            LexemTable.Add("[", (int)LexemeCodes.OPEN_BRACKET, true);
            LexemTable.Add("]", (int)LexemeCodes.CLOSE_BRACKET, true);
            LexemTable.Add("+", (int)LexemeCodes.PLUS, true);
            LexemTable.Add("-", (int)LexemeCodes.MINUS, true);
            LexemTable.Add("*", (int)LexemeCodes.MULTIPLY, true);
            LexemTable.Add("/", (int)LexemeCodes.DIVIDE, true);
            LexemTable.Add(";", (int)LexemeCodes.SEMICOLON, true);
            LexemTable.Add(",", (int)LexemeCodes.COMMA, true);
            LexemTable.Add(":=", (int)LexemeCodes.ASSIGNMENT);
            LexemTable.Add("=", (int)LexemeCodes.EQUAL, true);
            LexemTable.Add(">", (int)LexemeCodes.MORE, true);
            LexemTable.Add("<", (int)LexemeCodes.LESS, true);
            LexemTable.Add(">=", (int)LexemeCodes.MORE_OR_EQUAL);
            LexemTable.Add("<=", (int)LexemeCodes.LESS_OR_EQUAL);
            LexemTable.Add("!=", (int)LexemeCodes.NOT_EQUAL);
            LexemTable.Add("id", (int)LexemeCodes.ID);
            LexemTable.Add("const", (int)LexemeCodes.CONST);
            LexemTable.Add("string", (int)LexemeCodes.STRING);
            LexemTable.LastKeywordIndex = LexemTable.IndexOf("and");
            LexemTable.AddEmptyCharacter(' ');
            LexemTable.AddEmptyCharacter((char)13);
            
        }
        public LexemeAnalyzer()
        {
            IdentifierTable = new IdentifierTable();
            ConstantsTable = new ConstantsTable();
            StringTable = new StringTable();
            OutTable = new LexemeAnalyzerOutTable();
            IsError = false;
            ErrorLine = -1;
        }

        public LexemeAnalyzerOutTable PushCode(string code)
        {
            Automat auto; 
            try { 
                auto = new Automat(LexemTable, OutTable, IdentifierTable, ConstantsTable, StringTable);
                auto.PushString(code);
                if (!auto.IsDone)
                {
                    auto.PushChar(' ');
                }
            }
            catch(LexicalError ex)
            {
                IsError = true;
                ErrorLine = ex.Line;
                ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                IsError = true;
                ErrorMessage = ex.Message;
                ErrorLine = 0;
            }
            return OutTable;
        }
        public void ClearOutTable()
        {
            OutTable = new LexemeAnalyzerOutTable();
        }
    }
    public class LexicalError : Exception
    {
        public int Line
        {
            get; private set;
        }

        public LexicalError(string message, int line)
            :base(message)
        {
            Line = line;
        }
    }

}
