namespace NotesApp.UI
{
    partial class CategoriesForm
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
            NewCategoryLbl = new Label();
            NewSubcategoryLbl = new Label();
            NewCategoryTxt = new TextBox();
            NewSubcategoryTxt = new TextBox();
            CategoriesLbx = new ListBox();
            SubcategoriesLbx = new ListBox();
            AddCategoryBtn = new Button();
            AddSubcategoryBtn = new Button();
            SuspendLayout();
            // 
            // NewCategoryLbl
            // 
            NewCategoryLbl.AutoSize = true;
            NewCategoryLbl.ForeColor = Color.White;
            NewCategoryLbl.Location = new Point(28, 32);
            NewCategoryLbl.Margin = new Padding(4, 0, 4, 0);
            NewCategoryLbl.Name = "NewCategoryLbl";
            NewCategoryLbl.Size = new Size(134, 38);
            NewCategoryLbl.TabIndex = 0;
            NewCategoryLbl.Text = "Category:";
            // 
            // NewSubcategoryLbl
            // 
            NewSubcategoryLbl.AutoSize = true;
            NewSubcategoryLbl.ForeColor = Color.White;
            NewSubcategoryLbl.Location = new Point(452, 32);
            NewSubcategoryLbl.Name = "NewSubcategoryLbl";
            NewSubcategoryLbl.Size = new Size(177, 38);
            NewSubcategoryLbl.TabIndex = 1;
            NewSubcategoryLbl.Text = "Subcategory:";
            // 
            // NewCategoryTxt
            // 
            NewCategoryTxt.BorderStyle = BorderStyle.FixedSingle;
            NewCategoryTxt.Location = new Point(28, 82);
            NewCategoryTxt.Name = "NewCategoryTxt";
            NewCategoryTxt.Size = new Size(409, 45);
            NewCategoryTxt.TabIndex = 2;
            // 
            // NewSubcategoryTxt
            // 
            NewSubcategoryTxt.BorderStyle = BorderStyle.FixedSingle;
            NewSubcategoryTxt.Location = new Point(452, 82);
            NewSubcategoryTxt.Name = "NewSubcategoryTxt";
            NewSubcategoryTxt.Size = new Size(404, 45);
            NewSubcategoryTxt.TabIndex = 3;
            // 
            // CategoriesLbx
            // 
            CategoriesLbx.ForeColor = Color.White;
            CategoriesLbx.FormattingEnabled = true;
            CategoriesLbx.ItemHeight = 38;
            CategoriesLbx.Location = new Point(28, 144);
            CategoriesLbx.Name = "CategoriesLbx";
            CategoriesLbx.Size = new Size(409, 422);
            CategoriesLbx.TabIndex = 4;
            CategoriesLbx.SelectedIndexChanged += CategoriesLbx_SelectedIndexChanged;
            // 
            // SubcategoriesLbx
            // 
            SubcategoriesLbx.ForeColor = Color.White;
            SubcategoriesLbx.FormattingEnabled = true;
            SubcategoriesLbx.ItemHeight = 38;
            SubcategoriesLbx.Location = new Point(452, 144);
            SubcategoriesLbx.Name = "SubcategoriesLbx";
            SubcategoriesLbx.Size = new Size(404, 422);
            SubcategoriesLbx.TabIndex = 5;
            // 
            // AddCategoryBtn
            // 
            AddCategoryBtn.FlatAppearance.BorderSize = 0;
            AddCategoryBtn.FlatStyle = FlatStyle.Flat;
            AddCategoryBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AddCategoryBtn.Location = new Point(28, 586);
            AddCategoryBtn.Name = "AddCategoryBtn";
            AddCategoryBtn.Size = new Size(404, 61);
            AddCategoryBtn.TabIndex = 6;
            AddCategoryBtn.Text = "Add category";
            AddCategoryBtn.UseVisualStyleBackColor = true;
            AddCategoryBtn.Click += AddCategoryBtn_Click;
            // 
            // AddSubcategoryBtn
            // 
            AddSubcategoryBtn.FlatAppearance.BorderSize = 0;
            AddSubcategoryBtn.FlatStyle = FlatStyle.Flat;
            AddSubcategoryBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AddSubcategoryBtn.Location = new Point(452, 586);
            AddSubcategoryBtn.Name = "AddSubcategoryBtn";
            AddSubcategoryBtn.Size = new Size(404, 61);
            AddSubcategoryBtn.TabIndex = 7;
            AddSubcategoryBtn.Text = "Add subcategory";
            AddSubcategoryBtn.UseVisualStyleBackColor = true;
            AddSubcategoryBtn.Click += AddSubcategoryBtn_Click;
            // 
            // CategoriesForm
            // 
            AutoScaleDimensions = new SizeF(15F, 38F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 66, 91);
            ClientSize = new Size(885, 666);
            Controls.Add(AddSubcategoryBtn);
            Controls.Add(AddCategoryBtn);
            Controls.Add(SubcategoriesLbx);
            Controls.Add(CategoriesLbx);
            Controls.Add(NewSubcategoryTxt);
            Controls.Add(NewCategoryTxt);
            Controls.Add(NewSubcategoryLbl);
            Controls.Add(NewCategoryLbl);
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 5, 4, 5);
            Name = "CategoriesForm";
            Text = "Add categories";
            Load += CategoriesForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label NewCategoryLbl;
        private Label NewSubcategoryLbl;
        private TextBox NewCategoryTxt;
        private TextBox NewSubcategoryTxt;
        private ListBox CategoriesLbx;
        private ListBox SubcategoriesLbx;
        private Button AddCategoryBtn;
        private Button AddSubcategoryBtn;
    }
}