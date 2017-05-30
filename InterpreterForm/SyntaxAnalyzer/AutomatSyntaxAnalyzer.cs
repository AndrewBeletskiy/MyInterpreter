using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    class SyntaxAutomat
    {
        private int state_;
        private Stack<int> stack_;
        public bool IsDone { get; private set; }
        private LexemeAnalyzerOutTableElement CurrentLexeme
        {
            get; set;
        }
        
        public SyntaxAutomat()
        {
            stack_ = new Stack<int>();
            state_ = 1;
            IsDone = false;
        }
        public void PushLexeme(LexemeAnalyzerOutTableElement lexeme)
        {
            CurrentLexeme = lexeme;
            PushLexemeCode(lexeme.LexemeCode);
        }
        private void GenerateError(string formatString)
        {
            throw new Exception(string.Format(formatString, CurrentLexeme.StringNumber, CurrentLexeme.Substring));
        }
        private void Push(int nextState)
        {
            stack_.Push(nextState);
        }
        private void Out()
        {
            if (stack_.Count == 0)
            { 
                IsDone = true;
            }
            else
            {
                state_ = stack_.Pop();
            }
        }
        private void PushLexemeCode(int code)
        {
            switch (state_)
            {
                case 1:
                    if (code == 12) // {
                    { 
                        state_ = 3; // <operator list>
                        Push(2);
                    } else
                    {
                        GenerateError(SyntaxErrors.PROG_ERROR_BEGIN);
                    }
                    break;
                case 2:
                    if (code == 13) // }
                    {
                        Out();
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.PROG_ERROR_END);
                    }
                    break;
                case 3:
                    state_ = 6;
                    Push(4);
                    PushLexeme(CurrentLexeme);
                    break;
                case 4:
                    if (code == 22) // ;
                    {
                        state_ = 5;
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.OP_LIST_SEMICOLON_NOT_EXIST);
                    }
                    break;
                case 5:
                    switch (code)
                    { 
                        case 13: // }
                        case 8: // end
                            Out();
                            PushLexeme(CurrentLexeme);
                            break;
                        default:
                            state_ = 6;
                            Push(4);
                            PushLexeme(CurrentLexeme);
                            //GenerateError(SyntaxErrors.OP_ILLEGAL_OPERATOR);
                            break;
                    }
                    break;
                case 6:
                    switch (code)
                    {
                        case 31: // id
                            state_ = 7;
                            break;
                        case 1: // read
                            state_ = 9;
                            break;
                        case 2: // write
                            state_ = 12;
                            break;
                        case 3: // if
                            state_ = 25;
                            Push(15);
                            break;
                        case 6: // do
                            state_ = 17;
                            break;
                        default:
                            GenerateError(SyntaxErrors.OP_ILLEGAL_OPERATOR);
                            break;
                    }
                    break;
                case 7:
                    if (code == 24) // :=
                    {
                        state_ = 22;
                        Push(8);
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.ASSIGNMENT_SIGN_NOT_EXIST);
                    }
                    break;
                case 8:
                    Out();
                    PushLexeme(CurrentLexeme);
                    break;
                case 9:
                    if (code == 14) // (
                    {
                        state_ = 10;
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.OPEN_BRACKET_NOT_EXIST);
                    }
                    break;
                case 10:
                    if (code == 31) // id
                    {
                        state_ = 11;
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.ID_EXPECTED);
                    }
                    break;
                case 11:
                    switch (code)
                    {
                        case 23: // ,
                            state_ = 10;
                            break;
                        case 15: // )
                            Out();
                            break;
                        default:
                            GenerateError(SyntaxErrors.CLOSE_BRACKET_NOT_EXIST);
                            break;
                    }
                    break;
                case 12:
                    if (code == 14) // (
                    {
                        state_ = 13;
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.OPEN_BRACKET_NOT_EXIST);
                    }
                    break;
                case 13:
                    switch (code)
                    {
                        case 31: // id
                        case 33: // str
                            state_ = 14;
                            break;
                        default:
                            GenerateError(SyntaxErrors.STR_OR_ID_EXPECTED);
                            break;
                    }
                    break;
                case 14:
                    switch (code)
                    {
                        case 23: // ,
                            state_ = 13;
                            break;
                        case 15: // )
                            Out();
                            break;
                        default:
                            GenerateError(SyntaxErrors.CLOSE_BRACKET_NOT_EXIST);
                            break;
                    }
                    break;
                case 15:
                    if (code == 4) // then
                    {
                        state_ = 6;
                        Push(16);
                    } else
                    {
                        GenerateError(SyntaxErrors.THEN_NOT_EXIST);
                    }
                    break;
                case 16:
                    if (code == 5) // else
                    {
                        state_ = 6;
                        Push(8);
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.ELSE_NOT_EXIST);
                    }
                    break;
                case 17:
                    if (code == 7) // while
                    {
                        state_ = 18;
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.WHILE_NOT_EXIST);
                    }
                    break;
                case 18:
                    if (code == 14) // (
                    {
                        state_ = 25;
                        Push(19);
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.OPEN_BRACKET_NOT_EXIST);
                    }
                    break;
                case 19:
                    if (code == 15) // )
                    {
                        state_ = 20;
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.CLOSE_BRACKET_NOT_EXIST);
                    }
                    break;
                case 20:
                    if (code == 22) // ;
                    {
                        state_ = 3;
                        Push(21);
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.WHILE_SEMICOLON_NOT_EXIST);
                    }
                    break;
                case 21:
                    if (code == 8) // end 
                    {
                        Out();
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.END_NOT_EXIST);
                    }
                    break;
                case 22:
                    switch (code)
                    {
                        case 31: // id
                        case 32: // const
                            state_ = 23;
                            break;
                        case 14: // (
                            state_ = 22;
                            Push(24);
                            break;
                        default:
                            GenerateError(SyntaxErrors.ILLEGAL_EXPRESSION);
                            break;
                    }
                    break;
                case 23:
                    switch (code)
                    {
                        case 18: // +
                        case 19: // -
                        case 20: // *
                        case 21: // /
                            state_ = 22;
                            break;
                        default:
                            Out();
                            PushLexeme(CurrentLexeme);
                            break;
                    }
                    break;
                case 24:
                    if (code == 15) // )
                    {
                        state_ = 23;
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.CLOSE_BRACKET_NOT_EXIST);
                    }
                    break;
                case 25:
                    switch (code)
                    {
                        case 10: // not
                            state_ = 25;
                            break;
                        case 16: /// [
                            state_ = 25;
                            Push(28);
                            break;
                        default:
                            state_ = 22;
                            Push(26);
                            PushLexeme(CurrentLexeme);
                            //GenerateError(SyntaxErrors.ILLEGAL_LOGIC_EXPRESSION);
                            break;
                    }
                    break;
                case 26:
                    if (code >= 25 && code <= 30) // > < >= <= != =
                    {
                        state_ = 22;
                        Push(27);
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.ILLEGAL_LOGIC_EXPRESSION);
                    }
                    break;
                case 27:
                    switch (code)
                    {
                        case 11: // and
                        case 9: // or
                            state_ = 25;
                            break;
                        default:
                            Out();
                            PushLexeme(CurrentLexeme);
                            break;
                    }
                    break;
                case 28:
                    if (code == 17) // ]
                    {
                        state_ = 27;
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.CLOSE_BRACKET_NOT_EXIST);
                    }
                    break;
                   
            }
        }

    }
    class AutomatSyntaxAnalyzer : ISyntaxAnalyzer
    {
        LexemeAnalyzerOutTable table_;
        public int ErrorLine
        {
            get; private set;
        }

        public AutomatSyntaxAnalyzer()
        {
            ErrorLine = -1;
            table_ = null;
        }
        public AutomatSyntaxAnalyzer(LexemeAnalyzerOutTable table)
        {
            table_ = table;
        }
        public string ErrorMessage
        {
            get; private set;
        }

        public bool Test()
        {
            if (table_ == null)
            {
                ErrorMessage = "Lexeme table doesn't exist.";
                return false;
            }
            SyntaxAutomat automat = new SyntaxAutomat();
            try { 
                for (var i = 0; i < table_.Length; i++)
                {
                    automat.PushLexeme(table_[i]);
                }
                if (!automat.IsDone)
                {
                    ErrorMessage = "Unexpected end of file";
                    return false;
                }
                return true;
            } catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool Test(LexemeAnalyzerOutTable table)
        {
            table_ = table;
            return Test();
        }
    }
}
