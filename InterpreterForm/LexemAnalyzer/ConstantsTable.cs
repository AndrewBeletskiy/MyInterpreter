using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    class ConstantsTableElement
    {
        public double Value { get; private set; }
        private int index_;
        public int Index
        {
            get { return index_; }
            set
            {
                if (value >= 0)
                    index_ = value;
                else
                    throw new Exception("Index of constant must be greater than 0");
            }
        }
        public ConstantsTableElement(double value, int index)
        {
            Value = value;
            Index = index;
        }
    }
    class ConstantsTable
    {
        List<ConstantsTableElement> table_;
        public double this[int index]
        {
            get
            {
                return table_[index].Value;
            }
        }
        public ConstantsTable()
        {
            table_ = new List<ConstantsTableElement>();
        }
        public int IndexOf(double value)
        {
            //ConstantsTableElement founded = table_.Find(elem => elem.Value == value);
            //return (founded == null) ? 0 : founded.Index;
            return table_.FindIndex(elem => elem.Value == value);
        }
        public int Add(double value)
        {
            int indexOfValue = IndexOf(value);
            if (indexOfValue == -1)
            {
                indexOfValue = table_.Count;
                table_.Add(new ConstantsTableElement(value, table_.Count));
            }
            return indexOfValue;
        }
        public void ShowConsole()
        {
            ColoredTable table = new ColoredTable(table_.Count + 1, 2, " | ");
            table.AddString("Constant", 0, 0, ConsoleColor.White, ConsoleColor.DarkMagenta);
            table.AddString("Number", 0, 1, ConsoleColor.White, ConsoleColor.DarkMagenta);
            for (var i = 0; i < table_.Count; i++)
            {
                table.AddString(table_[i].Value.ToString(), i + 1, 0);
                table.AddString(table_[i].Index.ToString(), i + 1, 1);
            }
            table.Normalize();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("OUT TABLE OF CONSTANTS: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            table.Show();
        }
        public object GetDataGridList()
        {
            return (from number in table_
                    select new { Value = number.Value, Index = number.Index }).ToList();
        }
    }
}
