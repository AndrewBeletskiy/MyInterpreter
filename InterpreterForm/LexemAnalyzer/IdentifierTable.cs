using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterForm
{
    class IdentifierTableElement
    {
        public string Name { get; private set; }
        private int index_;
        public double Value { get; set; }
        public int Index {
            get { return index_; }
            private set
            {
                if (value > 0)
                    index_ = value;
                else
                    throw new Exception("Index of identifier must be greater than 0.");
            }
        }

        public IdentifierTableElement(string name, int index)
        {
            Name = name;
            Index = index;
            Value = 0;
        }
    }
    class IdentifierTable
    {
        List<IdentifierTableElement> table_;
        public IdentifierTableElement this[int index]
        {
            get
            {
                return table_[index];
            }
        }
        public int Length => table_.Count;
        public IdentifierTable()
        {
            table_ = new List<IdentifierTableElement>();
        }
        public int GetCodeOf(string id)
        {
            IdentifierTableElement founded = table_.Find(elem => elem.Name == id);
            return (founded == null) ? 0 : founded.Index;
        }
        public int Add(string newIdentifier)
        {
            int index = 0;
            if (table_.Find(elem => elem.Name == newIdentifier) == null)
            {
                index = table_.Count + 1;
                table_.Add(new IdentifierTableElement(newIdentifier, table_.Count + 1));
                return index;
            } else
            {
                throw new Exception(String.Format("\"{0}\" already added into the identifier table", newIdentifier));
            }
        }
        public void ShowConsole()
        {
            ColoredTable table = new ColoredTable(table_.Count + 1, 2, " | ");
            table.AddString("Identifier", 0, 0, ConsoleColor.White, ConsoleColor.DarkBlue);
            table.AddString("Number", 0, 1, ConsoleColor.White, ConsoleColor.DarkBlue);
            for (var i = 0; i < table_.Count; i++)
            {
                table.AddString(table_[i].Name, i + 1, 0);
                table.AddString(table_[i].Index.ToString(), i + 1, 1);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("OUT TABLE OF IDENTIFIER: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            table.Show();
        }
        public object GetDataGridList()
        {
            return (from id in table_
                    select new { Name = id.Name, Index = id.Index}).ToList();
        }
    }
}
