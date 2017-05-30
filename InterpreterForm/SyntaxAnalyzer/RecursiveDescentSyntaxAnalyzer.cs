using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    class RecursiveDescentSyntaxAnalyzer : ISyntaxAnalyzer
    {
        LexemeAnalyzerOutTable table;
        private int Current;
        private LexemeAnalyzerOutTableElement CurrentLexem
        {
            get
            {
                if (Current < table.Length)
                    return table[Current];

                var lastLexem = table[table.Length - 1];
                throw new Exception(String.Format(SyntaxErrors.END_OF_FILE, lastLexem.StringNumber));
            }
        }
        private int CurrentCode
        {
            get
            {
                if (CurrentLexem != null)
                    return CurrentLexem.LexemeCode;
                return 0;
            }
        }
        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
            }
            
        }
        public int ErrorLine { get; private set; }

        public RecursiveDescentSyntaxAnalyzer(LexemeAnalyzerOutTable table)
        {
            this.table = table;
            Current = 0;
            errorMessage = "There is no error.";
            ErrorLine = -1;
        }
        public RecursiveDescentSyntaxAnalyzer() : this(null)
        {

        }

        public bool Test(LexemeAnalyzerOutTable table)
        {
            this.table = table;
            return Test();
        }

        public bool Test()
        {
            if (table == null)
                return false;

            Initialize();
            ErrorLine = -1;
            try
            { 
                return program();
            }
            catch(LexicalError err)
            {
                ErrorMessage = err.Message;
                ErrorLine = err.Line;
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }
        private void Initialize()
        {
            ErrorMessage = "There is no error.";
            Current = 0;
        }

        private bool program()
        {
            bool founded = false;
            if (CurrentCode == 12) // {
            {
                Current++;
                if (op_list()) // <сп. оп>
                {
                    if (CurrentCode == 13) // }
                    {
                        Current++;
                        founded = true;
                    }
                    else {
                        GenerateError(SyntaxErrors.PROG_ERROR_END);
                    }
                }
            }
            else
            {
                GenerateError(SyntaxErrors.PROG_ERROR_BEGIN);
            }
                
            return founded;
        }

        private bool op_list(bool isInWhile = false)
        {
            bool founded = false;
            if (op()) // <оп>
            {
                if (CurrentCode == 22) // ;
                {
                    Current++;
                    founded = true;
                    while (founded && ((!isInWhile && CurrentCode != 13) // }
                                    || (isInWhile && CurrentCode != 8))) // end
                    {
                        if (op()) // <оп>
                        {
                            if (CurrentCode == 22) // ;
                            { 
                                Current++;
                            }
                            else
                            {
                                founded = false;
                                GenerateError(SyntaxErrors.OP_LIST_SEMICOLON_NOT_EXIST);
                            }
                        }
                        else
                        {
                            founded = false;
                        }
                    }
                }
                else
                {
                    GenerateError(SyntaxErrors.OP_LIST_SEMICOLON_NOT_EXIST);
                }
            }
            return founded;
        }
        private bool op()
        {
            bool founded = false;
            switch (CurrentCode)
            {
                case 31: // id
                    Current++;
                    if (CurrentCode == 24) // :=
                    {
                        Current++;
                        if (expr()) // <выр>
                        {
                            founded = true;
                        }
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.ASSIGNMENT_SIGN_NOT_EXIST);
                    }
                    break;
                case 1: // read
                    Current++;
                    if (CurrentCode == 14) // (
                    {
                        Current++;
                        if (id_list()) // <сп. ид>
                        {
                            if (CurrentCode == 15) //)
                            {
                                Current++;
                                founded = true;
                            }
                            else
                            {
                                GenerateError(SyntaxErrors.CLOSE_BRACKET_NOT_EXIST);
                            }
                        }
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.OPEN_BRACKET_NOT_EXIST);
                    }
                    break;
                case 2:  // write
                    Current++;
                    if (CurrentCode == 14) // (
                    {
                        Current++;
                        if (output_list()) // <сп. выв>
                        {
                            if (CurrentCode == 15) //)
                            {
                                Current++;
                                founded = true;
                            }
                            else
                            {
                                GenerateError(SyntaxErrors.CLOSE_BRACKET_NOT_EXIST);
                            }
                        }
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.OPEN_BRACKET_NOT_EXIST);
                    }
                    break;
                case 3: // if
                    Current++;
                    if (logic_expr()) // <lv>
                    {
                        if (CurrentCode == 4) // then
                        {
                            Current++;
                            if (op()) // <op>
                            {
                                if (CurrentCode == 5) // else
                                {
                                    Current++;
                                    if (op()) // <op>
                                    {
                                        founded = true;
                                    }
                                }
                                else
                                {
                                    GenerateError(SyntaxErrors.ELSE_NOT_EXIST);
                                }
                            }
                        }
                        else
                        {
                            GenerateError(SyntaxErrors.THEN_NOT_EXIST);
                        }
                    }
                    break;
                case 6: // do
                    Current++;
                    if (CurrentCode == 7) // while
                    {
                        Current++;
                        if (CurrentCode == 14) // (
                        {
                            Current++;
                            if (logic_expr()) // <лв>
                            {
                                if (CurrentCode == 15) // )
                                {
                                    Current++;
                                    if (CurrentCode == 22) // ;
                                    {
                                        Current++;
                                        if (op_list(true)) // <сп. оп>
                                        {
                                            if (CurrentCode == 8) // end
                                            {
                                                Current++;
                                                founded = true;
                                            }
                                            else
                                            {
                                                GenerateError(SyntaxErrors.END_NOT_EXIST);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        GenerateError(SyntaxErrors.WHILE_SEMICOLON_NOT_EXIST);
                                    }
                                }
                                else
                                {
                                    GenerateError(SyntaxErrors.CLOSE_BRACKET_NOT_EXIST);
                                }
                            }
                        }
                        else
                        {
                            GenerateError(SyntaxErrors.OPEN_BRACKET_NOT_EXIST);
                        }
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.WHILE_NOT_EXIST);
                    }
                    break;
                default:
                    GenerateError(SyntaxErrors.OP_ILLEGAL_OPERATOR);
                    break;

            }
            return founded;
        }

        private bool id_list()
        {
            bool founded = false;
            if (CurrentCode == 31) // id
            {
                Current++;
                founded = true;
                while (founded && CurrentCode == 23) // ,
                {
                    Current++;
                    if (CurrentCode == 31) // id
                    {
                        Current++;
                    } else
                    {
                        GenerateError(SyntaxErrors.ID_EXPECTED);
                        founded = false;
                    }
                }
            }
            else
            {
                GenerateError(SyntaxErrors.ID_EXPECTED);
            }
            return founded;
        }

        private bool output_list()
        {
            bool founded = false;
            if (output_elem()) // 
            {
                founded = true;
                while (founded && CurrentCode == 23) // ,
                {
                    Current++;
                    if (!output_elem()) // <елем. вывода>
                    {
                        founded = false;
                    }
                }
            }
            return founded;
        }
        private bool output_elem()
        {
            if (CurrentCode == 31 || CurrentCode == 33)
            {
                Current++;
                return true;
            }
            GenerateError(SyntaxErrors.STR_OR_ID_EXPECTED);
            return false;
        }

        private bool expr()
        {
            bool founded = false;
            if (term()) // <терм>
            {
                founded = true;
                while (founded && (CurrentCode == 18 || CurrentCode == 19)) // + || - 
                {
                    Current++;
                    if (!term()) // <терм>
                    {                       
                        founded = false;
                    }
                }
            }
            return founded;
        }
        private bool term()
        {
            bool founded = false;
            if (mnoj()) // <множ>
            {
                founded = true;
                while (founded && (CurrentCode == 20 || CurrentCode == 21)) // * || /
                {
                    Current++;
                    if (!mnoj()) // <множ>
                    {
                        founded = false;
                    }
                }
            }
            return founded;
        }
        private bool mnoj()
        {
            bool founded = false;
            if (CurrentCode == 31 || CurrentCode == 32) // id || const
            {
                Current++;
                founded = true;
            }
            else if (CurrentCode == 14) // (
            {
                Current++;
                if (expr()) // <выр>
                {
                    if (CurrentCode == 15) // )
                    {
                        Current++;
                        founded = true;
                    }
                    else
                    {
                        GenerateError(SyntaxErrors.ILLEGAL_EXPRESSION);
                    }
                }
            }
            if (!founded)
                GenerateError(SyntaxErrors.ILLEGAL_EXPRESSION);
            
            return founded;
        }

        private bool logic_expr()
        {
            bool founded = false;
            if (logic_term()) // <лт>
            {
                founded = true;
                while (founded && CurrentCode == 9) // or
                {
                    Current++;
                    if (!logic_term()) // <лт>
                    {
                        founded = false;
                    }
                }
            }
            return founded;
        }
        private bool logic_term()
        {
            bool founded = false;
            if (logic_mnoj()) // <лм>
            {
                founded = true;
                while (founded && CurrentCode == 11) // and
                {
                    Current++;
                    if (!logic_mnoj()) // <лм>
                    {
                        founded = false;
                    }
                }
            }
            return founded;
        }
        private bool logic_mnoj()
        {
            bool founded = false;
            if (CurrentCode == 16) // [
            {
                Current++;
                if (logic_expr()) // <лв>
                {
                    if (CurrentCode == 17) // ]
                    {
                        Current++;
                        founded = true;
                    }
                }
            }
            else if (CurrentCode == 10) // not
            {
                Current++;
                if (logic_mnoj())
                {
                    founded = true;
                }
            }
            else if (relation()) // 
            { 
               founded = true;     
            }
            if (!founded)
                GenerateError(SyntaxErrors.ILLEGAL_LOGIC_EXPRESSION);
            return founded;
        }
        private bool relation()
        {
            bool founded = false;
            if (expr())
            {
                if (CurrentCode >= 25 && CurrentCode <= 30) // > | < | >= | <= | = | !=
                {
                    Current++;
                    if (expr())
                    {
                        founded = true;
                    }
                }
                else
                {
                    GenerateError(SyntaxErrors.ILLEGAL_RELATION_SIGN);
                }
            }
            return founded;
        }


        private void GenerateError(string formatString)
        {
            throw new LexicalError(string.Format(formatString, CurrentLexem.StringNumber, CurrentLexem.Substring), CurrentLexem.StringNumber);
        }

    }
}
