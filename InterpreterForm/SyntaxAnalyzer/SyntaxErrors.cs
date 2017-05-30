using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    static class SyntaxErrors
    {
        public static string PROG_ERROR_BEGIN = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Программа должна начинаться с символа '{'.";
        public static string PROG_ERROR_END = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Программа должна заканчиваться символом '}'.";
        public static string OP_LIST_SEMICOLON_NOT_EXIST = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Оператор должен оканчиваться символом ';'";
        public static string OP_ILLEGAL_OPERATOR= "Ошибка в строке: {0}. Недопустимая лексема: {1}. Оператор неопределён.";
        public static string ASSIGNMENT_SIGN_NOT_EXIST = "Ошибка в строке: {0}. Недопустимая лексема: {1}. В присваивании ожидается ':='.";
        public static string CLOSE_BRACKET_NOT_EXIST = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Ожидается закрывающая скобка ')'.";
        public static string OPEN_BRACKET_NOT_EXIST = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Ожидается открывающая скобка '('.";
        public static string ELSE_NOT_EXIST = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Ожидается ключевое слово 'else'.";
        public static string THEN_NOT_EXIST = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Ожидается ключевое слово 'then'.";
        public static string END_NOT_EXIST = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Ожидается ключевое слово 'end'.";
        public static string WHILE_SEMICOLON_NOT_EXIST = "Ошибка в строке: {0}. Недопустимая лексема: {1}. После условия в цикле нужно поставить ';'.";
        public static string WHILE_NOT_EXIST = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Отсутствует ключевое слово 'while' в описании цикла.";
        public static string ID_EXPECTED = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Ожидается идентификатор.";
        public static string STR_OR_ID_EXPECTED = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Ожидается идентификатор или строка.";
        public static string ILLEGAL_LOGIC_EXPRESSION = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Неверное логическое выражение";
        public static string ILLEGAL_EXPRESSION = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Неверное арифметическое выражение";
        public static string ILLEGAL_RELATION_SIGN = "Ошибка в строке: {0}. Недопустимая лексема: {1}. Недопустимый знак сравнения.";
        public static string END_OF_FILE = "Ошибка в строке {0}. Неожиданный конец файла.";
    }
}
