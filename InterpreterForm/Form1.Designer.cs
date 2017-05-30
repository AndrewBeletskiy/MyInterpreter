
namespace InterpreterForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.runTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxCode = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.dataOutLexemTable = new System.Windows.Forms.DataGridView();
            this.dataOutStringTable = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataOutNumberTable = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataOutIdTable = new System.Windows.Forms.DataGridView();
            this.dataInLexemTable = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.polizBrowser = new System.Windows.Forms.WebBrowser();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.enterButton = new System.Windows.Forms.Button();
            this.inputLabel = new System.Windows.Forms.Label();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьФайлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выйтиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeState = new System.Windows.Forms.StatusStrip();
            this.state = new System.Windows.Forms.ToolStripStatusLabel();
            this.runTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataOutLexemTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataOutStringTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataOutNumberTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataOutIdTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInLexemTable)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.codeState.SuspendLayout();
            this.SuspendLayout();
            // 
            // runTab
            // 
            this.runTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runTab.Controls.Add(this.tabPage1);
            this.runTab.Controls.Add(this.tabPage5);
            this.runTab.Controls.Add(this.tabPage4);
            this.runTab.Controls.Add(this.tabPage2);
            this.runTab.Location = new System.Drawing.Point(12, 27);
            this.runTab.Name = "runTab";
            this.runTab.SelectedIndex = 0;
            this.runTab.Size = new System.Drawing.Size(778, 393);
            this.runTab.TabIndex = 0;
            this.runTab.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxCode);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(770, 367);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Редактор";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxCode
            // 
            this.textBoxCode.AcceptsTab = true;
            this.textBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCode.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.textBoxCode.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxCode.ForeColor = System.Drawing.Color.LightGray;
            this.textBoxCode.Location = new System.Drawing.Point(0, 0);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(770, 367);
            this.textBoxCode.TabIndex = 0;
            this.textBoxCode.Text = "{\n\ta:= (62 - 20) * 6;\n\tb:= (10 + 32) * 7;\n\tdo while [a > 0] and [b > 0]\n\t\tif [a >" +
    " b]\n\t\t\tthen a := a - b\n\t\t\telse b := b - a \n\t\tend;\n\tend;\n}";
            this.textBoxCode.TextChanged += new System.EventHandler(this.textCode_TextChanged);
            this.textBoxCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCode_KeyDown);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.dataOutLexemTable);
            this.tabPage2.Controls.Add(this.dataOutStringTable);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.dataOutNumberTable);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.dataOutIdTable);
            this.tabPage2.Controls.Add(this.dataInLexemTable);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(770, 367);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Лексический Анализатор";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(436, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Выходная таблица лексем";
            // 
            // dataOutLexemTable
            // 
            this.dataOutLexemTable.AllowUserToAddRows = false;
            this.dataOutLexemTable.AllowUserToDeleteRows = false;
            this.dataOutLexemTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataOutLexemTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataOutLexemTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataOutLexemTable.Location = new System.Drawing.Point(439, 19);
            this.dataOutLexemTable.Name = "dataOutLexemTable";
            this.dataOutLexemTable.ReadOnly = true;
            this.dataOutLexemTable.RowHeadersVisible = false;
            this.dataOutLexemTable.Size = new System.Drawing.Size(325, 355);
            this.dataOutLexemTable.TabIndex = 8;
            // 
            // dataOutStringTable
            // 
            this.dataOutStringTable.AllowUserToAddRows = false;
            this.dataOutStringTable.AllowUserToDeleteRows = false;
            this.dataOutStringTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataOutStringTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataOutStringTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataOutStringTable.Location = new System.Drawing.Point(187, 269);
            this.dataOutStringTable.Name = "dataOutStringTable";
            this.dataOutStringTable.ReadOnly = true;
            this.dataOutStringTable.RowHeadersVisible = false;
            this.dataOutStringTable.Size = new System.Drawing.Size(246, 105);
            this.dataOutStringTable.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(187, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(207, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Выходная таблица строковых констант";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Выходная таблица числовых констант";
            // 
            // dataOutNumberTable
            // 
            this.dataOutNumberTable.AllowUserToAddRows = false;
            this.dataOutNumberTable.AllowUserToDeleteRows = false;
            this.dataOutNumberTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataOutNumberTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataOutNumberTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataOutNumberTable.Location = new System.Drawing.Point(187, 145);
            this.dataOutNumberTable.Name = "dataOutNumberTable";
            this.dataOutNumberTable.ReadOnly = true;
            this.dataOutNumberTable.RowHeadersVisible = false;
            this.dataOutNumberTable.Size = new System.Drawing.Size(246, 105);
            this.dataOutNumberTable.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Выходная таблица идентификторов";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Входная таблица лексем";
            // 
            // dataOutIdTable
            // 
            this.dataOutIdTable.AllowUserToAddRows = false;
            this.dataOutIdTable.AllowUserToDeleteRows = false;
            this.dataOutIdTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataOutIdTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataOutIdTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataOutIdTable.Location = new System.Drawing.Point(187, 19);
            this.dataOutIdTable.Name = "dataOutIdTable";
            this.dataOutIdTable.ReadOnly = true;
            this.dataOutIdTable.RowHeadersVisible = false;
            this.dataOutIdTable.Size = new System.Drawing.Size(246, 105);
            this.dataOutIdTable.TabIndex = 1;
            // 
            // dataInLexemTable
            // 
            this.dataInLexemTable.AllowUserToAddRows = false;
            this.dataInLexemTable.AllowUserToDeleteRows = false;
            this.dataInLexemTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataInLexemTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataInLexemTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataInLexemTable.Location = new System.Drawing.Point(6, 19);
            this.dataInLexemTable.Name = "dataInLexemTable";
            this.dataInLexemTable.ReadOnly = true;
            this.dataInLexemTable.RowHeadersVisible = false;
            this.dataInLexemTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataInLexemTable.Size = new System.Drawing.Size(175, 355);
            this.dataInLexemTable.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.polizBrowser);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(770, 367);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Полиз";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // polizBrowser
            // 
            this.polizBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.polizBrowser.Location = new System.Drawing.Point(3, 3);
            this.polizBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.polizBrowser.Name = "polizBrowser";
            this.polizBrowser.Size = new System.Drawing.Size(764, 361);
            this.polizBrowser.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.enterButton);
            this.tabPage5.Controls.Add(this.inputLabel);
            this.tabPage5.Controls.Add(this.inputTextBox);
            this.tabPage5.Controls.Add(this.outputTextBox);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(770, 367);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Выполнить";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // enterButton
            // 
            this.enterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.enterButton.Location = new System.Drawing.Point(658, 341);
            this.enterButton.Name = "enterButton";
            this.enterButton.Size = new System.Drawing.Size(106, 23);
            this.enterButton.TabIndex = 3;
            this.enterButton.Text = "Ввод";
            this.enterButton.UseVisualStyleBackColor = true;
            this.enterButton.Click += new System.EventHandler(this.enterButton_Click);
            // 
            // inputLabel
            // 
            this.inputLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputLabel.AutoSize = true;
            this.inputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inputLabel.Location = new System.Drawing.Point(6, 344);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(85, 16);
            this.inputLabel.TabIndex = 2;
            this.inputLabel.Text = "Поле ввода";
            // 
            // inputTextBox
            // 
            this.inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputTextBox.Location = new System.Drawing.Point(97, 343);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(555, 20);
            this.inputTextBox.TabIndex = 1;
            // 
            // outputTextBox
            // 
            this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTextBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.outputTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.outputTextBox.ForeColor = System.Drawing.SystemColors.Window;
            this.outputTextBox.Location = new System.Drawing.Point(0, 0);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.Size = new System.Drawing.Size(770, 336);
            this.outputTextBox.TabIndex = 0;
            this.outputTextBox.Text = "Program started:\n";
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(802, 24);
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьФайлToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.сохранитьКакToolStripMenuItem,
            this.выйтиToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьФайлToolStripMenuItem
            // 
            this.открытьФайлToolStripMenuItem.Name = "открытьФайлToolStripMenuItem";
            this.открытьФайлToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.открытьФайлToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.открытьФайлToolStripMenuItem.Text = "Открыть файл";
            this.открытьФайлToolStripMenuItem.Click += new System.EventHandler(this.открытьФайлToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // сохранитьКакToolStripMenuItem
            // 
            this.сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            this.сохранитьКакToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.сохранитьКакToolStripMenuItem.Text = "Сохранить как...";
            this.сохранитьКакToolStripMenuItem.Click += new System.EventHandler(this.сохранитьКакToolStripMenuItem_Click);
            // 
            // выйтиToolStripMenuItem
            // 
            this.выйтиToolStripMenuItem.Name = "выйтиToolStripMenuItem";
            this.выйтиToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.выйтиToolStripMenuItem.Text = "Выйти";
            this.выйтиToolStripMenuItem.Click += new System.EventHandler(this.выйтиToolStripMenuItem_Click);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // codeState
            // 
            this.codeState.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.state});
            this.codeState.Location = new System.Drawing.Point(0, 423);
            this.codeState.Name = "codeState";
            this.codeState.Size = new System.Drawing.Size(802, 22);
            this.codeState.TabIndex = 2;
            this.codeState.Text = "statusStrip1";
            // 
            // state
            // 
            this.state.Name = "state";
            this.state.Size = new System.Drawing.Size(66, 17);
            this.state.Text = "Состояние";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 445);
            this.Controls.Add(this.codeState);
            this.Controls.Add(this.runTab);
            this.Controls.Add(this.menuMain);
            this.Name = "Form1";
            this.Text = "Interpreter";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.runTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataOutLexemTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataOutStringTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataOutNumberTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataOutIdTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInLexemTable)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.codeState.ResumeLayout(false);
            this.codeState.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl runTab;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьФайлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выйтиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataOutLexemTable;
        private System.Windows.Forms.DataGridView dataOutStringTable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataOutNumberTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataOutIdTable;
        private System.Windows.Forms.DataGridView dataInLexemTable;
        private System.Windows.Forms.StatusStrip codeState;
        private System.Windows.Forms.ToolStripStatusLabel state;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox textBoxCode;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.WebBrowser polizBrowser;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button enterButton;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.RichTextBox outputTextBox;
    }
}

