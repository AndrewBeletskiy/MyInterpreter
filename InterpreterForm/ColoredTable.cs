using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
namespace InterpreterForm
{
    public class IllegalTerminatorValueException : Exception
    {
        public IllegalTerminatorValueException(string value)
            : base(String.Format("Недопустимое значение разделителя: {0}", value))
        {
        }
    }
    /// <summary>
    /// Клас, що дозволяє створювати кольорові текстові таблиці найменшої ширини, 
    /// що змінюється в залежності від довжини рядків у цій таблиці.
    /// </summary>
    class ColoredTable
    {
        public enum AlignType
        {
            Left, Right, Center, Justify
        }
        protected string[,] table;
        private int rows;
        private int columns;
        private string terminator;
        public AlignType Align { get; set; }
        ConsoleColor[,] colors;
        ConsoleColor[,] backgroundColors;
        public string this[int i,int j]
        {
            get
            {
                return table[i, j];
            }
            set
            {
                table[i, j] = value;
            }
        }

        public ConsoleColor[,] Colors
        {
            get { return colors; }
        }

        public ConsoleColor[,] BackgroundColors
        {
            get { return backgroundColors; }
        }

        public string Terminator
        {
            get
            {
                return terminator;
            }
            set
            {
                if (value != null && !value.Contains('\n') && !value.Contains('\0'))
                {
                    terminator = value;
                }
                else
                {
                    throw new IllegalTerminatorValueException(value);
                }
            }

        }

        public int Rows
        {
            get { return rows; }
        }

        public int Columns
        {
            get { return columns; }
        }

        public ColoredTable(int rows, int columns) : this(rows, columns, " ")
        { }
        public ColoredTable(int rows, int columns, string terminator)
        {
            this.rows = rows;
            this.columns = columns;
            table = new string[rows, columns];
            Terminator = terminator;
            colors = new ConsoleColor[rows, columns];
            backgroundColors = new ConsoleColor[rows, columns];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    colors[i, j] = ConsoleColor.Gray;
                    backgroundColors[i, j] = ConsoleColor.Black;
                }
            }
            this.Align = AlignType.Left;
        }


        public void ClearTable()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    table[i, j] = "";
                }
            }
        }
        private void trimAllCells()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    table[row, column] = table[row, column].Trim();
                    while (table[row, column].Contains("  "))
                    {
                        table[row, column] = table[row, column].Replace("  ", " ");
                    }
                }
            }
        }
        /// <summary>
        /// Робить рядки у кожному стовпчику одної довжини.
        /// </summary>
        public void Normalize()
        {
            trimAllCells();
            for (int col = 0; col < columns; col++)
            {
                int maxLength = table[0, col].Length;

                for (int i = 1; i < rows; i++)
                {
                    if (maxLength < table[i, col].Length)
                    {
                        maxLength = table[i, col].Length;
                    }
                }

                StringBuilder temp;
                for (int i = 0; i < rows; i++)
                {
                    int delta = maxLength - table[i, col].Length;

                    switch (Align)
                    {
                        case AlignType.Left:
                            temp = new StringBuilder().Append(table[i, col]).Append(new string(' ', delta));
                            break;
                        case AlignType.Right:
                            temp = new StringBuilder().Append(new string(' ', delta)).Append(table[i, col]);
                            break;
                        case AlignType.Center:
                            temp = new StringBuilder()
                                        .Append(new string(' ', delta / 2))
                                        .Append(table[i, col])
                                        .Append(new string(' ', delta - delta / 2));
                            break;
                        case AlignType.Justify:
                            string value = table[i, col];
                            int spaceCount = value.Count(elem => char.IsWhiteSpace(elem));
                            if (spaceCount > 0)
                            { 
                                int forEachSpace = delta / spaceCount;
                                delta -= forEachSpace * spaceCount;
                                temp = new StringBuilder();
                                for (var j = 0; j < value.Length; j++)
                                {
                                    temp.Append(value[j]);
                                    if (char.IsWhiteSpace(value[j]))
                                    {
                                        temp.Append(new String(' ', forEachSpace));
                                        if (delta > 0)
                                        {
                                            temp.Append(' ');
                                            delta--;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                temp = new StringBuilder().Append(table[i, col]).Append(new string(' ', delta));
                            }
                            break;
                        default:
                            temp = new StringBuilder().Append(table[i, col]).Append(new string(' ', delta));
                            break;
                    }                   

                    table[i, col] = temp.ToString();
                }
            }
        }
        /// <summary>
        /// Робить рядки у кожній клітинці одної довжини.
        /// </summary>

        public string getContent(int row, int column)
        {
            return getRawContent(row, column) + Terminator;
        }
        public string getRawContent(int row, int column)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                return table[row, column];
            }
            else
            {
                throw new IndexOutOfRangeException(String.Format("Недопустимая пара индексов: {0},{1}", row, column));
            }
        }

        public void AddString(string str, int row, int column, ConsoleColor color = ConsoleColor.Gray, ConsoleColor background = ConsoleColor.Black)
        {
            if (str != null)
            {
                table[row, column] = str;
                colors[row, column] = color;
                backgroundColors[row, column] = background;
            }
            else
            {
                throw new ArgumentException();
            }

        }

        public override string ToString()
        {
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    temp.Append(table[i, j]);
                    temp.Append(Terminator);
                }
                temp.Append('\n');
            }
            temp.Remove(temp.Length - 1, 1);
            return temp.ToString();
        }
        public void Show()
        {
            Normalize();
            ConsoleColor oldColor = Console.ForegroundColor;
            ConsoleColor oldBackground = Console.BackgroundColor;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.ForegroundColor = Colors[i, j];
                    Console.BackgroundColor = BackgroundColors[i, j];
                    Console.Write(getContent(i, j));                      
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = oldColor;
            Console.BackgroundColor = oldBackground;
        }

        public static ColoredTable Create(ICollection list,
                                          string terminator = " ",
                                          ConsoleColor headerColor = ConsoleColor.Gray,
                                          ConsoleColor headerBackground = ConsoleColor.Black,
                                          ConsoleColor color = ConsoleColor.Gray,
                                          ConsoleColor background = ConsoleColor.Black)
        {
            if (list == null || list.Count == 0)
                throw new Exception("List must be not empty");
            object first = null;
            foreach (var item in list)
            {
                first = item;
                break;
            }
            Type elemType = first.GetType();
            string[] publicMemberNames = (from member in elemType.GetMembers()
                                          where (member.MemberType == MemberTypes.Field) || (member.MemberType == MemberTypes.Property)
                                          select member.Name).ToArray<string>();
            ColoredTable resultTable = new ColoredTable(list.Count + 1, publicMemberNames.Length, terminator);
            for (var i = 0; i < publicMemberNames.Length; i++)
            {
                resultTable.AddString(publicMemberNames[i], 0, i, headerColor, headerBackground);
            }
            int currentRow = 1;
            foreach (var item in list)
            {
                for (var i = 0; i < publicMemberNames.Length; i++)
                {
                    FieldInfo field = elemType.GetField(publicMemberNames[i]);
                    PropertyInfo prop = elemType.GetProperty(publicMemberNames[i]);
                    string strValue = "";
                    if (prop != null)
                    {
                        strValue = prop.GetValue(item).ToString();
                    }
                    else if (field != null)
                    {
                        strValue = field.GetValue(item).ToString();
                    }

                    resultTable.AddString(strValue, currentRow, i, color, background);

                }
                currentRow++;
            }
            return resultTable;
        }
        
    }
}