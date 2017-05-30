using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    interface ISyntaxAnalyzer
    {
        bool Test(LexemeAnalyzerOutTable table);
        bool Test();
        int ErrorLine { get; }
        string ErrorMessage { get; }
    }
}
