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
            CategoryFilterCbx = new ComboBox();
            SubcategoryFilterCbx = new ComboBox();
            TitleLbl = new Label();
            NoteTxt = new RichTextBox();
            EditKnowledgeLevelBtn = new Button();
            LeftPanel = new Panel();
            RightPanel = new Panel();
            NoteTitleLbl = new Label();
            ((System.ComponentModel.ISupportInitialize)NotesGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)KnowledgeLevelNum).BeginInit();
            LeftPanel.SuspendLayout();
            RightPanel.SuspendLayout();
            SuspendLayout();
            // 
            // NotesGrid
            // 
            NotesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            NotesGrid.Location = new Point(22, 147);
            NotesGrid.Margin = new Padding(3, 5, 3, 5);
            NotesGrid.Name = "NotesGrid";
            NotesGrid.RowHeadersWidth = 62;
            NotesGrid.Size = new Size(1060, 661);
            NotesGrid.TabIndex = 0;
            NotesGrid.RowsDefaultCellStyleChanged += NotesGrid_RowsDefaultCellStyleChanged;
            NotesGrid.CellClick += NotesGrid_CellClick;
            // 
            // NewNoteBtn
            // 
            NewNoteBtn.BackColor = Color.White;
            NewNoteBtn.FlatAppearance.BorderSize = 0;
            NewNoteBtn.FlatStyle = FlatStyle.Flat;
            NewNoteBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NewNoteBtn.ForeColor = Color.FromArgb(32, 45, 64);
            NewNoteBtn.Location = new Point(20, 753);
            NewNoteBtn.Margin = new Padding(4, 5, 4, 5);
            NewNoteBtn.Name = "NewNoteBtn";
            NewNoteBtn.Size = new Size(228, 55);
            NewNoteBtn.TabIndex = 1;
            NewNoteBtn.Text = "Add new note";
            NewNoteBtn.UseVisualStyleBackColor = false;
            NewNoteBtn.Click += NewNoteBtn_Click;
            // 
            // KnowledgeLevelNum
            // 
            KnowledgeLevelNum.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            KnowledgeLevelNum.Location = new Point(280, 753);
            KnowledgeLevelNum.Margin = new Padding(7, 8, 7, 8);
            KnowledgeLevelNum.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            KnowledgeLevelNum.Name = "KnowledgeLevelNum";
            KnowledgeLevelNum.Size = new Size(73, 55);
            KnowledgeLevelNum.TabIndex = 2;
            // 
            // CategoryFilterCbx
            // 
            CategoryFilterCbx.DropDownStyle = ComboBoxStyle.DropDownList;
            CategoryFilterCbx.FormattingEnabled = true;
            CategoryFilterCbx.Location = new Point(22, 17);
            CategoryFilterCbx.Name = "CategoryFilterCbx";
            CategoryFilterCbx.Size = new Size(1060, 46);
            CategoryFilterCbx.TabIndex = 4;
            CategoryFilterCbx.SelectedIndexChanged += CategoryFilterCbx_SelectedIndexChanged;
            // 
            // SubcategoryFilterCbx
            // 
            SubcategoryFilterCbx.DropDownStyle = ComboBoxStyle.DropDownList;
            SubcategoryFilterCbx.FormattingEnabled = true;
            SubcategoryFilterCbx.Location = new Point(22, 81);
            SubcategoryFilterCbx.Name = "SubcategoryFilterCbx";
            SubcategoryFilterCbx.Size = new Size(1060, 46);
            SubcategoryFilterCbx.TabIndex = 5;
            SubcategoryFilterCbx.DropDown += SubcategoryFilterCbx_DropDown;
            SubcategoryFilterCbx.SelectedIndexChanged += SubcategoryFilterCbx_SelectedIndexChanged;
            // 
            // TitleLbl
            // 
            TitleLbl.AutoSize = true;
            TitleLbl.Location = new Point(1082, 20);
            TitleLbl.Name = "TitleLbl";
            TitleLbl.Size = new Size(0, 38);
            TitleLbl.TabIndex = 6;
            // 
            // NoteTxt
            // 
            NoteTxt.Location = new Point(20, 81);
            NoteTxt.Name = "NoteTxt";
            NoteTxt.Size = new Size(584, 643);
            NoteTxt.TabIndex = 7;
            NoteTxt.Text = "";
            // 
            // EditKnowledgeLevelBtn
            // 
            EditKnowledgeLevelBtn.BackColor = Color.White;
            EditKnowledgeLevelBtn.FlatAppearance.BorderSize = 0;
            EditKnowledgeLevelBtn.FlatStyle = FlatStyle.Flat;
            EditKnowledgeLevelBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            EditKnowledgeLevelBtn.ForeColor = Color.FromArgb(32, 45, 64);
            EditKnowledgeLevelBtn.Location = new Point(376, 753);
            EditKnowledgeLevelBtn.Name = "EditKnowledgeLevelBtn";
            EditKnowledgeLevelBtn.Size = new Size(228, 55);
            EditKnowledgeLevelBtn.TabIndex = 8;
            EditKnowledgeLevelBtn.Text = "Edit knowledge";
            EditKnowledgeLevelBtn.UseVisualStyleBackColor = false;
            EditKnowledgeLevelBtn.Click += EditKnowledgeLevelBtn_Click;
            // 
            // LeftPanel
            // 
            LeftPanel.BackColor = Color.FromArgb(45, 66, 91);
            LeftPanel.Controls.Add(SubcategoryFilterCbx);
            LeftPanel.Controls.Add(CategoryFilterCbx);
            LeftPanel.Controls.Add(NotesGrid);
            LeftPanel.Dock = DockStyle.Left;
            LeftPanel.Location = new Point(0, 0);
            LeftPanel.Name = "LeftPanel";
            LeftPanel.Size = new Size(1098, 840);
            LeftPanel.TabIndex = 9;
            // 
            // RightPanel
            // 
            RightPanel.BackColor = Color.FromArgb(32, 45, 64);
            RightPanel.Controls.Add(NoteTitleLbl);
            RightPanel.Controls.Add(NoteTxt);
            RightPanel.Controls.Add(EditKnowledgeLevelBtn);
            RightPanel.Controls.Add(KnowledgeLevelNum);
            RightPanel.Controls.Add(NewNoteBtn);
            RightPanel.Dock = DockStyle.Fill;
            RightPanel.ForeColor = Color.White;
            RightPanel.Location = new Point(1098, 0);
            RightPanel.Name = "RightPanel";
            RightPanel.Size = new Size(633, 840);
            RightPanel.TabIndex = 10;
            // 
            // NoteTitleLbl
            // 
            NoteTitleLbl.AutoSize = true;
            NoteTitleLbl.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NoteTitleLbl.Location = new Point(20, 20);
            NoteTitleLbl.Name = "NoteTitleLbl";
            NoteTitleLbl.Size = new Size(135, 45);
            NoteTitleLbl.TabIndex = 9;
            NoteTitleLbl.Text = "+++++";
            // 
            // NotesForm
            // 
            AutoScaleDimensions = new SizeF(15F, 38F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1731, 840);
            Controls.Add(RightPanel);
            Controls.Add(TitleLbl);
            Controls.Add(LeftPanel);
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(3, 5, 3, 5);
            Name = "NotesForm";
            Text = "Notes";
            Load += NotesForm_Load;
            ((System.ComponentModel.ISupportInitialize)NotesGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)KnowledgeLevelNum).EndInit();
            LeftPanel.ResumeLayout(false);
            RightPanel.ResumeLayout(false);
            RightPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView NotesGrid;
        private Button NewNoteBtn;
        private NumericUpDown KnowledgeLevelNum;
        private ComboBox CategoryFilterCbx;
        private ComboBox SubcategoryFilterCbx;
        private Label TitleLbl;
        private RichTextBox NoteTxt;
        private Button EditKnowledgeLevelBtn;
        private Panel LeftPanel;
        private Panel RightPanel;
        private Label NoteTitleLbl;
    }
}