using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    class StringTableElement
    {
        public string Value { get; private set; }
        private int index_;
        public int Index
        {
            get { return index_; }
            set
            {
                if (value >= 0)
                    index_ = value;
                else
                    throw new Exception("Index of constant must be not less than 0");
            }
        }
        public StringTableElement(string value, int index)
        {
            Value = value;
            Index = index;
        }
    }
    class StringTable
    {
        List<StringTableElement> table_;
        public string this [int index] => table_[index].Value;
        public StringTable()
        {
            table_ = new List<StringTableElement>();
        }
        private int IndexOf(string value)
        {
            //StringTableElement founded = table_.Find(elem => elem.Value == value);
            //return (founded == null) ? 0 : founded.Index;
            return table_.FindIndex(elem => elem.Value == value);
        }
        public int Add(string value)
        {
            int indexOfValue = IndexOf(value);
            //if (indexOfValue == 0)
            if (indexOfValue == -1)
            {
                indexOfValue = table_.Count;
                table_.Add(new StringTableElement(value, table_.Count));
            }
            return indexOfValue;
        }
        public void ShowConsole()
        {
            ColoredTable table = new ColoredTable(table_.Count + 1, 2, " | ");
            table.AddString("String", 0, 0, ConsoleColor.White, ConsoleColor.DarkGreen);
            table.AddString("Number", 0, 1, ConsoleColor.White, ConsoleColor.DarkGreen);
            for (var i = 0; i < table_.Count; i++)
            {
                table.AddString('"'+table_[i].Value+'"', i + 1, 0);
                table.AddString(table_[i].Index.ToString(), i + 1, 1);
            }
            table.Normalize();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("OUT TABLE OF STRINGS: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            table.Show();
        }
        public object GetDataGridList()
        {
            return (from str in table_
                    select new { Value = str.Value, Index = str.Index }).ToList();
        }
    }
}
