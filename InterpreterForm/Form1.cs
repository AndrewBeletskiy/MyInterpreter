using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;



namespace InterpreterForm
{
    public partial class Form1 : Form
    {
        private static Color TextColor = Color.White;
        private static Color KeywordColor = Color.LightBlue;
        private static Color ParenthesesColor = Color.Yellow;
        private static Color IdColor = Color.LightGreen;
        private static Color StringColor = Color.Orange;
        private string currentFileName;
        private int ErrorLine = -1;
        private bool IsFormat;
        private bool TextSaved
        {
            get
            {
                if (!File.Exists(CurrentFileName))
                    return false;
                string[] file = File.ReadAllLines(CurrentFileName);
                string[] text = textBoxCode.Lines;
                
                if (file.Length != text.Length)
                    return false;
                for (var i = 0; i < file.Length; i++)
                {
                    if (!file[i].Equals(text[i]))
                        return false;
                }
                return true;
            }
        }
        private string CurrentFileName
        {
            get
            {
                return currentFileName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && File.Exists(value))
                {
                    currentFileName = value;
                    this.Text = String.Format("Interpreter({0})", Path.GetFileName(value));
                } 
            }
        }
        private string ProgramCode
        {
            get
            {
                return textBoxCode.Text;
            }
            set
            {
                textBoxCode.Text = value;
            }
        }
        private ISyntaxAnalyzer syntaxAnalyzer;
        
        public Form1()
        {
            InitializeComponent();
            currentFileName = "";
            textBoxCode.AcceptsTab = true;
            IsFormat = false;
            syntaxAnalyzer = new RecursiveDescentSyntaxAnalyzer();
            //FormatText();
            //syntaxAnalyzer = new AutomatSyntaxAnalyzer();
            
            //syntaxAnalyzer = new UpGoSyntaxAnalyzer();

        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            CurrentFileName = dialog.FileName;
            if (dialog.CheckFileExists)
                textBoxCode.Text = File.ReadAllText(CurrentFileName);
        }

        internal void Write(string substring)
        {
            outputTextBox.Text += substring;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFileName != "")
            {
                if (DialogResult.Yes == MessageBox.Show("Вы действительно хотите сохранить?", "Сохранение", MessageBoxButtons.YesNo))
                {
                    saveToCurrentFile();
                }
            } else
            {
                сохранитьКакToolStripMenuItem_Click(sender, e);
            }
        }
        private void saveToCurrentFile()
        {
            File.WriteAllLines(CurrentFileName, textBoxCode.Lines);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.ShowDialog();

            if (string.IsNullOrEmpty(saveDialog.FileName))
                return;
        
            File.WriteAllLines(saveDialog.FileName, textBoxCode.Lines);
            CurrentFileName = saveDialog.FileName;

        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Лабораторна робота з курсу \"Основи розробки трансляторів\". Варіант №5.\nАвтор:Білецький Андрій", "О программе", MessageBoxButtons.OK);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (runTab.SelectedIndex == 0)
                return;
            if (string.IsNullOrEmpty(ProgramCode))
            {
                MessageBox.Show("Отсутствует код программы.");
                return;
            }
            if (!TextSaved)
            {

                if (!string.IsNullOrEmpty(CurrentFileName) 
                    && DialogResult.Yes == MessageBox.Show("Хотите ли сохранить внесённые в код программы изменения, и использовать их при трансляции?", "Сохранить перед трансляцией?", MessageBoxButtons.YesNo))
                {
                    saveToCurrentFile();
                }
                else
                {
                    сохранитьКакToolStripMenuItem_Click(sender, e);
                }
                
            }
            if (runTab.SelectedIndex == 1)
            {
                runProgram();
            }
            else if (runTab.SelectedIndex == 2)
            {
                showPoliz();
            } else if (runTab.SelectedIndex == 3)
            {
                callLexemeAnalyzer();   
            }
        }
        private void runProgram()
        {
            outputTextBox.Text = "Program started: \n";
            string text = "";
            try
            {
                text = File.ReadAllText(CurrentFileName);
            }
            catch (Exception)
            {
                text = textBoxCode.Text;
            }

            LexemeAnalyzer lex = new LexemeAnalyzer();
            if (testCode(lex, text))
            {
                var poliz = new Poliz(lex.IdentifierTable, lex.StringTable, lex.ConstantsTable, lex.OutTable);
                Executor executor = new Executor(this, poliz);
                executor.Run();
            }
        }

        private void callLexemeAnalyzer()
        {
            LexemeAnalyzer lexemAnalyzer = new LexemeAnalyzer();
            string text = "";
            try
            {
                text = File.ReadAllText(CurrentFileName);
            } catch (Exception)
            {
                text = textBoxCode.Text;
            }
            lexemAnalyzer.PushCode(text);
            if (lexemAnalyzer.IsError)
            {
                MessageBox.Show(lexemAnalyzer.ErrorMessage, "Ошибка");
            } else
            {
                dataInLexemTable.DataSource = LexemeAnalyzer.LexemTable.GetDataGridList();
                dataOutIdTable.DataSource = lexemAnalyzer.IdentifierTable.GetDataGridList();
                dataOutNumberTable.DataSource = lexemAnalyzer.ConstantsTable.GetDataGridList();
                dataOutStringTable.DataSource = lexemAnalyzer.StringTable.GetDataGridList();
                dataOutLexemTable.DataSource = lexemAnalyzer.OutTable.GetDataGridList();
                dataOutLexemTable.Columns[0].HeaderText = "№";
                dataOutLexemTable.Columns[1].HeaderText = "Подстрока";
                dataOutLexemTable.Columns[2].HeaderText = "Код";
                dataOutLexemTable.Columns[3].HeaderText = "Номер";

            }
        }
        private void showPoliz()
        {
            string text = "";
            try
            {
                text = File.ReadAllText(CurrentFileName);
            }
            catch (Exception)
            {
                text = textBoxCode.Text;
            }

            LexemeAnalyzer lex = new LexemeAnalyzer();
            if (testCode(lex, text))
            {
                var poliz = new Poliz(lex.IdentifierTable, lex.StringTable, lex.ConstantsTable, lex.OutTable);
                polizBrowser.DocumentText = poliz.GetHTML();
            }

        }
        private bool testCode(LexemeAnalyzer lex, string text)
        {
            
            lex.PushCode(text);
            if (lex.IsError)
            {
                return false;
            }
            if (!syntaxAnalyzer.Test(lex.OutTable))
            {
                return false;
            }
                return true;
        }
        private void textCode_TextChanged(object sender, EventArgs e)
        {
            ErrorLine = -1;
            
            if (string.IsNullOrWhiteSpace(ProgramCode))
            {
                codeState.Items[0].ForeColor = Color.Black;
                codeState.Items[0].Text = "Код отсутствует";
                return;
            }
            LexemeAnalyzer lexemAnalyzer = new LexemeAnalyzer();
            
            lexemAnalyzer.PushCode(ProgramCode);
            if (!lexemAnalyzer.IsError)
            {
                if (syntaxAnalyzer.Test(lexemAnalyzer.OutTable))
                {
                    codeState.Items[0].ForeColor = Color.Green;
                    codeState.Items[0].Text = "Код правильный";
                }
                else
                {
                    codeState.Items[0].ForeColor = Color.Red;
                    codeState.Items[0].Text = String.Format("Ошибка во время синтаксического анализа! {0}", syntaxAnalyzer.ErrorMessage);
                    ErrorLine = syntaxAnalyzer.ErrorLine;
                }
                
            } 
            else
            {
                codeState.Items[0].ForeColor = Color.Red;
                codeState.Items[0].Text = String.Format("Ошибка во время лексического анализа! {0}", lexemAnalyzer.ErrorMessage);
                ErrorLine = lexemAnalyzer.ErrorLine;
                //codeState.Items[0].Text = String.Format("Lexical error: {0}", lexemAnalyzer.ErrorMessage);

            }
            if (IsFormat)
                FormatText();
        }

        
        private void FormatText()
        {
            int startCursor = textBoxCode.SelectionStart;
            ResetColors();
            SetIdColor();
            string text = textBoxCode.Text;
            setKeyword("read");
            setKeyword("write");
            setKeyword("if");
            setKeyword("then");
            setKeyword("else");
            setKeyword("end");
            setKeyword("or");
            setKeyword("and");
            setKeyword("not");
            setKeyword("do");
            setKeyword("while");
            setKeyword("end");

            setColor("(", ParenthesesColor);
            setColor(")", ParenthesesColor);
            setColor("[", ParenthesesColor);
            setColor("]", ParenthesesColor);

            setStringColor();
            ShowErrorLine();
            textBoxCode.SelectionStart = startCursor;
            textBoxCode.SelectionLength = 0;
            textBoxCode.SelectionColor = TextColor;
        }
        private void ShowErrorLine()
        {
            if (ErrorLine - 1<0 || ErrorLine - 1>= textBoxCode.Lines.Length)
                return;
            int sum = 0;
            for (var i = 0; i < ErrorLine-1; i++)
            {
                sum += textBoxCode.Lines[i].Length+1;
            }
            textBoxCode.SelectionStart = sum;
            int len = textBoxCode.Lines[ErrorLine - 1].Length;
            len = (len + sum < textBoxCode.Text.Length) ? len : textBoxCode.Text.Length - sum;
            textBoxCode.SelectionLength = textBoxCode.Lines[ErrorLine - 1].Length;
            textBoxCode.SelectionFont = new Font(textBoxCode.Font, FontStyle.Underline);


        }
        private void setColor(string keyword, Color color)
        {
            int i = 0;
            while (i < ProgramCode.Length && (i = ProgramCode.IndexOf(keyword, i)) >= 0)
            {
                textBoxCode.SelectionStart = i;
                textBoxCode.SelectionLength = keyword.Length;
                textBoxCode.SelectionColor = color;
                textBoxCode.SelectionLength = 0;
                i++;
            }
        }
        private bool TestKeyword(string keyword, int i)
        {
            if (i > 0)
            {
                if (!char.IsWhiteSpace(ProgramCode[i - 1]))
                {
                    return false;
                }
            }
            if (i + keyword.Length < ProgramCode.Length)
            {
                if (char.IsLetterOrDigit(ProgramCode[i + keyword.Length]))
                    return false;
            }
            return true;
        }
        private void setKeyword(string keyword)
        {
            int i = 0;
            while (i < ProgramCode.Length && (i = ProgramCode.IndexOf(keyword, i)) >= 0)
            {
                if (TestKeyword(keyword, i))
                {
                    textBoxCode.SelectionStart = i;
                    textBoxCode.SelectionLength = keyword.Length;
                    textBoxCode.SelectionColor = KeywordColor;
                    textBoxCode.SelectionLength = 0;
                }
                i++;
            }
        }
        private void ResetColors()
        {
            textBoxCode.SelectionStart = 0;
            textBoxCode.SelectionLength = ProgramCode.Length;
            textBoxCode.SelectionFont = textBoxCode.Font;
            textBoxCode.SelectionColor = TextColor;
        }
        private void SetIdColor()
        {
            int i = 0;

            do
            {
                while (i < ProgramCode.Length && !char.IsLetter(ProgramCode[i++])) ;
                if (i == ProgramCode.Length)
                    return;
                textBoxCode.SelectionStart = (i - 1) > 0 ? i - 1 : 0;
                while (i < ProgramCode.Length && char.IsLetterOrDigit(ProgramCode[i++])) ;
                if (i == ProgramCode.Length)
                    return;
                textBoxCode.SelectionLength = i - textBoxCode.SelectionStart - 1;
                textBoxCode.SelectionColor = IdColor;

            } while (i < ProgramCode.Length);
        }
        private void setStringColor()
        {
            setColor("\"\"", StringColor);
            int i = 0;

            do
            {
                while (i < ProgramCode.Length && ProgramCode[i++] != '"') ;
                if (i == ProgramCode.Length)
                    return;
                textBoxCode.SelectionStart = (i - 1) > 0 ? i - 1 : 0;
                i++;
                while (i < ProgramCode.Length && ProgramCode[i++] != '"') ;
                if (i == ProgramCode.Length)
                    return;
                textBoxCode.SelectionLength = (i - textBoxCode.SelectionStart >= 2) ? i - textBoxCode.SelectionStart : 2;

                textBoxCode.SelectionColor = StringColor;

            } while (i < ProgramCode.Length);
        }

        private void textBoxCode_KeyDown(object sender, KeyEventArgs e)
        {
            int startPos = textBoxCode.SelectionStart;
            if (e.KeyCode == Keys.D9 && e.Shift)
            {
                textBoxCode.Text = textBoxCode.Text.Insert(textBoxCode.SelectionStart, ")");
            }
            if (e.KeyCode == Keys.OemOpenBrackets && !e.Shift)
            {
                textBoxCode.Text = textBoxCode.Text.Insert(textBoxCode.SelectionStart, "]");
            }
            if (e.KeyCode == Keys.OemQuotes && e.Shift)
            {
                textBoxCode.Text = textBoxCode.Text.Insert(textBoxCode.SelectionStart, "\"");
            }
            if(e.Control && e.KeyCode == Keys.H)
            {
                IsFormat = !IsFormat;
            }
            if (e.KeyCode == Keys.F5)
            {
                if (runTab.SelectedIndex != 1) { 
                    runTab.SelectedIndex = 1;
                } else
                {
                    runProgram();
                }
            }
            textBoxCode.SelectionStart = startPos;
        }
        Queue<string> InputStack = new Queue<string>();
        public string GetInput()
        {
            if (InputStack.Count == 0)
            {
                return null;
            } else
            {
                return InputStack.Dequeue();
            }
        }
        private void enterButton_Click(object sender, EventArgs e)
        {
            InputStack.Enqueue(inputTextBox.Text);
            outputTextBox.Text += inputTextBox.Text + "\n";
            inputTextBox.Text = "";
            Entered();
        }
        public event EnteredEventDelegat Entered;
        public delegate void EnteredEventDelegat();

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
    
}
