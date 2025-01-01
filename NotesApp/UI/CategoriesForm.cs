using Data.Interfaces;
using Data.Repositories;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotesApp.UI
{
    public partial class CategoriesForm : Form
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubcategoriesRepository _subcategoriesRepository;

        private Category _category;

        public CategoriesForm(ICategoriesRepository categoriesRepository, ISubcategoriesRepository subcategoriesRepository)
        {
            InitializeComponent();
            _categoriesRepository = categoriesRepository;
            _subcategoriesRepository = subcategoriesRepository;
        }

        private void CategoriesForm_Load(object sender, EventArgs e)
        {
            RefreshCategoryList();
            RefreshSubcategoryList();
        }

        private async void RefreshCategoryList()
        {
            CategoriesLbx.DataSource = await _categoriesRepository.SelectCategories();
            CategoriesLbx.DisplayMember = "Name";
        }

        private async void RefreshSubcategoryList()
        {
            SubcategoriesLbx.DataSource = await _subcategoriesRepository.SelectSubcategories();
            SubcategoriesLbx.DisplayMember = "Name";
        }

        private async void AddCategoryBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NewCategoryTxt.Text)) return;

            Category newCategory = new Category();
            newCategory.Name = NewCategoryTxt.Text;

            await _categoriesRepository.InsertCategory(newCategory);
            RefreshCategoryList();
            NewCategoryTxt.Text = "";
        }

        private async void AddSubcategoryBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NewSubcategoryTxt.Text)) return;

            Subcategory newSubcategory = new Subcategory();
            newSubcategory.Name = NewSubcategoryTxt.Text;
            newSubcategory.CategoryId = _category.Id;
          
            await _subcategoriesRepository.InsertSubcategory(newSubcategory);
            RefreshSubcategoryList();
            NewSubcategoryTxt.Text = "";
        }

        private void CategoriesLbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lbx = (ListBox)sender;
            _category = (Category)lbx.Items[lbx.SelectedIndex];
        }
    }
}
