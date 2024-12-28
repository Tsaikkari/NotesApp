namespace NotesApp
{
    partial class CreateOrEditNoteForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TitleLbl = new Label();
            CategoryLbl = new Label();
            SubcategoryLbl = new Label();
            TextLbl = new Label();
            EditNoteBtn = new Button();
            AddNoteBtn = new Button();
            TitleTxtBox = new TextBox();
            CategoryCbx = new ComboBox();
            SubcategoryCbx = new ComboBox();
            AddCategoryBtn = new Button();
            AddSubcategoryBtn = new Button();
            NoteTxt = new RichTextBox();
            SuspendLayout();
            // 
            // TitleLbl
            // 
            TitleLbl.AutoSize = true;
            TitleLbl.Location = new Point(18, 32);
            TitleLbl.Margin = new Padding(4, 0, 4, 0);
            TitleLbl.Name = "TitleLbl";
            TitleLbl.Size = new Size(76, 38);
            TitleLbl.TabIndex = 0;
            TitleLbl.Text = "Title:";
            // 
            // CategoryLbl
            // 
            CategoryLbl.AutoSize = true;
            CategoryLbl.Location = new Point(18, 93);
            CategoryLbl.Margin = new Padding(4, 0, 4, 0);
            CategoryLbl.Name = "CategoryLbl";
            CategoryLbl.Size = new Size(134, 38);
            CategoryLbl.TabIndex = 1;
            CategoryLbl.Text = "Category:";
            // 
            // SubcategoryLbl
            // 
            SubcategoryLbl.AutoSize = true;
            SubcategoryLbl.Location = new Point(18, 156);
            SubcategoryLbl.Margin = new Padding(4, 0, 4, 0);
            SubcategoryLbl.Name = "SubcategoryLbl";
            SubcategoryLbl.Size = new Size(177, 38);
            SubcategoryLbl.TabIndex = 2;
            SubcategoryLbl.Text = "Subcategory:";
            // 
            // TextLbl
            // 
            TextLbl.AutoSize = true;
            TextLbl.Location = new Point(18, 231);
            TextLbl.Margin = new Padding(4, 0, 4, 0);
            TextLbl.Name = "TextLbl";
            TextLbl.Size = new Size(72, 38);
            TextLbl.TabIndex = 3;
            TextLbl.Text = "Text:";
            // 
            // EditNoteBtn
            // 
            EditNoteBtn.Location = new Point(227, 614);
            EditNoteBtn.Margin = new Padding(4);
            EditNoteBtn.Name = "EditNoteBtn";
            EditNoteBtn.Size = new Size(643, 66);
            EditNoteBtn.TabIndex = 4;
            EditNoteBtn.Text = "Edit note";
            EditNoteBtn.UseVisualStyleBackColor = true;
            EditNoteBtn.Click += EditNoteBtn_Click;
            // 
            // AddNoteBtn
            // 
            AddNoteBtn.Location = new Point(227, 609);
            AddNoteBtn.Margin = new Padding(4);
            AddNoteBtn.Name = "AddNoteBtn";
            AddNoteBtn.Size = new Size(643, 71);
            AddNoteBtn.TabIndex = 5;
            AddNoteBtn.Text = "Add note";
            AddNoteBtn.UseVisualStyleBackColor = true;
            AddNoteBtn.Click += AddNoteBtn_Click;
            // 
            // TitleTxtBox
            // 
            TitleTxtBox.Location = new Point(227, 29);
            TitleTxtBox.Name = "TitleTxtBox";
            TitleTxtBox.Size = new Size(643, 45);
            TitleTxtBox.TabIndex = 6;
            // 
            // CategoryCbx
            // 
            CategoryCbx.FormattingEnabled = true;
            CategoryCbx.Location = new Point(227, 93);
            CategoryCbx.Name = "CategoryCbx";
            CategoryCbx.Size = new Size(489, 46);
            CategoryCbx.TabIndex = 8;
            // 
            // SubcategoryCbx
            // 
            SubcategoryCbx.FormattingEnabled = true;
            SubcategoryCbx.Location = new Point(227, 156);
            SubcategoryCbx.Name = "SubcategoryCbx";
            SubcategoryCbx.Size = new Size(489, 46);
            SubcategoryCbx.TabIndex = 9;
            // 
            // AddCategoryBtn
            // 
            AddCategoryBtn.Location = new Point(740, 93);
            AddCategoryBtn.Name = "AddCategoryBtn";
            AddCategoryBtn.Size = new Size(130, 46);
            AddCategoryBtn.TabIndex = 10;
            AddCategoryBtn.Text = "Add";
            AddCategoryBtn.UseVisualStyleBackColor = true;
            AddCategoryBtn.Click += AddCategoryBtn_Click;
            // 
            // AddSubcategoryBtn
            // 
            AddSubcategoryBtn.Location = new Point(740, 156);
            AddSubcategoryBtn.Name = "AddSubcategoryBtn";
            AddSubcategoryBtn.Size = new Size(130, 46);
            AddSubcategoryBtn.TabIndex = 11;
            AddSubcategoryBtn.Text = "Add";
            AddSubcategoryBtn.UseVisualStyleBackColor = true;
            AddSubcategoryBtn.Click += AddSubcategoryBtn_Click;
            // 
            // NoteTxt
            // 
            NoteTxt.Location = new Point(227, 228);
            NoteTxt.Name = "NoteTxt";
            NoteTxt.Size = new Size(643, 361);
            NoteTxt.TabIndex = 12;
            NoteTxt.Text = "";
            // 
            // CreateNote
            // 
            AutoScaleDimensions = new SizeF(15F, 38F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(897, 705);
            Controls.Add(NoteTxt);
            Controls.Add(AddSubcategoryBtn);
            Controls.Add(AddCategoryBtn);
            Controls.Add(SubcategoryCbx);
            Controls.Add(CategoryCbx);
            Controls.Add(TitleTxtBox);
            Controls.Add(AddNoteBtn);
            Controls.Add(EditNoteBtn);
            Controls.Add(TextLbl);
            Controls.Add(SubcategoryLbl);
            Controls.Add(CategoryLbl);
            Controls.Add(TitleLbl);
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "CreateNote";
            Text = "Note";
            Load += CreateNote_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label TitleLbl;
        private Label CategoryLbl;
        private Label SubcategoryLbl;
        private Label TextLbl;
        private Button EditNoteBtn;
        private Button AddNoteBtn;
        private TextBox TitleTxtBox;
        private ComboBox CategoryCbx;
        private ComboBox SubcategoryCbx;
        private Button AddCategoryBtn;
        private Button AddSubcategoryBtn;
        private RichTextBox NoteTxt;
    }
}
