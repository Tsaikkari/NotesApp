namespace NotesApp.UI
{
    partial class NotesForm
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
            NotesGrid = new DataGridView();
            NewNoteBtn = new Button();
            KnowledgeLevelNum = new NumericUpDown();
            KnowledgeLbl = new Label();
            CategoryFilterCbx = new ComboBox();
            SubcategoryFilterCbx = new ComboBox();
            TitleLbl = new Label();
            NoteTxt = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)NotesGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)KnowledgeLevelNum).BeginInit();
            SuspendLayout();
            // 
            // NotesGrid
            // 
            NotesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            NotesGrid.Location = new Point(18, 126);
            NotesGrid.Margin = new Padding(3, 5, 3, 5);
            NotesGrid.Name = "NotesGrid";
            NotesGrid.RowHeadersWidth = 62;
            NotesGrid.Size = new Size(1023, 658);
            NotesGrid.TabIndex = 0;
            NotesGrid.CellClick += NotesGrid_CellClick;
            // 
            // NewNoteBtn
            // 
            NewNoteBtn.Location = new Point(1082, 729);
            NewNoteBtn.Margin = new Padding(4, 5, 4, 5);
            NewNoteBtn.Name = "NewNoteBtn";
            NewNoteBtn.Size = new Size(198, 55);
            NewNoteBtn.TabIndex = 1;
            NewNoteBtn.Text = "Add new note";
            NewNoteBtn.UseVisualStyleBackColor = true;
            NewNoteBtn.Click += NewNoteBtn_Click;
            // 
            // KnowledgeLevelNum
            // 
            KnowledgeLevelNum.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            KnowledgeLevelNum.Location = new Point(1587, 729);
            KnowledgeLevelNum.Margin = new Padding(7, 8, 7, 8);
            KnowledgeLevelNum.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
            KnowledgeLevelNum.Name = "KnowledgeLevelNum";
            KnowledgeLevelNum.Size = new Size(128, 55);
            KnowledgeLevelNum.TabIndex = 2;
            // 
            // KnowledgeLbl
            // 
            KnowledgeLbl.AutoSize = true;
            KnowledgeLbl.Location = new Point(1351, 737);
            KnowledgeLbl.Margin = new Padding(4, 0, 4, 0);
            KnowledgeLbl.Name = "KnowledgeLbl";
            KnowledgeLbl.Size = new Size(225, 38);
            KnowledgeLbl.TabIndex = 3;
            KnowledgeLbl.Text = "Knowledge level:";
            // 
            // CategoryFilterCbx
            // 
            CategoryFilterCbx.FormattingEnabled = true;
            CategoryFilterCbx.Location = new Point(18, 12);
            CategoryFilterCbx.Name = "CategoryFilterCbx";
            CategoryFilterCbx.Size = new Size(1023, 46);
            CategoryFilterCbx.TabIndex = 4;
            CategoryFilterCbx.SelectedIndexChanged += CategoryFilterCbx_SelectedIndexChanged;
            // 
            // SubcategoryFilterCbx
            // 
            SubcategoryFilterCbx.FormattingEnabled = true;
            SubcategoryFilterCbx.Location = new Point(18, 64);
            SubcategoryFilterCbx.Name = "SubcategoryFilterCbx";
            SubcategoryFilterCbx.Size = new Size(1023, 46);
            SubcategoryFilterCbx.TabIndex = 5;
            SubcategoryFilterCbx.SelectedIndexChanged += SubcategoryFilterCbx_SelectedIndexChanged;
            // 
            // TitleLbl
            // 
            TitleLbl.AutoSize = true;
            TitleLbl.Location = new Point(1082, 12);
            TitleLbl.Name = "TitleLbl";
            TitleLbl.Size = new Size(28, 38);
            TitleLbl.TabIndex = 6;
            TitleLbl.Text = "/";
            // 
            // NoteTxt
            // 
            NoteTxt.Location = new Point(1082, 126);
            NoteTxt.Name = "NoteTxt";
            NoteTxt.Size = new Size(633, 577);
            NoteTxt.TabIndex = 7;
            NoteTxt.Text = "";
            // 
            // NotesForm
            // 
            AutoScaleDimensions = new SizeF(15F, 38F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1923, 818);
            Controls.Add(NoteTxt);
            Controls.Add(TitleLbl);
            Controls.Add(SubcategoryFilterCbx);
            Controls.Add(CategoryFilterCbx);
            Controls.Add(KnowledgeLbl);
            Controls.Add(KnowledgeLevelNum);
            Controls.Add(NewNoteBtn);
            Controls.Add(NotesGrid);
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(3, 5, 3, 5);
            Name = "NotesForm";
            Text = "Notes";
            Load += NotesForm_Load;
            ((System.ComponentModel.ISupportInitialize)NotesGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)KnowledgeLevelNum).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView NotesGrid;
        private Button NewNoteBtn;
        private NumericUpDown KnowledgeLevelNum;
        private Label KnowledgeLbl;
        private ComboBox CategoryFilterCbx;
        private ComboBox SubcategoryFilterCbx;
        private Label TitleLbl;
        private RichTextBox NoteTxt;
    }
}