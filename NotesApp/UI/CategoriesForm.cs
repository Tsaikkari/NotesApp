using Data.CustomQueryResults;
using Data.Interfaces;
using Data.Repositories;
using DomainModel.Models;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.Categories;
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
    public enum CategoryType { Default, Selected };
    public partial class CategoriesForm : Form
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubcategoriesRepository _subcategoriesRepository;
        private readonly IServiceProvider _serviceProvider;

        private Category _category;
        CategoriesCache _categoriesCache;

        public CategoriesForm(ICategoriesRepository categoriesRepository, ISubcategoriesRepository subcategoriesRepository, IServiceProvider serviceProvider, INotesRepository notesRepository)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _categoriesRepository = categoriesRepository;
            _subcategoriesRepository = subcategoriesRepository;
            _categoriesCache = _serviceProvider.GetRequiredService<CategoriesCache>();
        }

        private void RefreshCategoryList()
        {
            CategoriesLbx.DataSource = _categoriesCache.DefaultCategoryList;
            CategoriesLbx.DisplayMember = "Name";
        }

        private void RefreshSubcategoryList()
        {
            SubcategoriesLbx.DataSource = _categoriesCache.DefaultSubcategoryList;
            SubcategoriesLbx.DisplayMember = "Name";
        }

        private async void CategoriesForm_Load(object sender, EventArgs e)
        {
            await _categoriesCache.RefreshData();
            RefreshCategoryList();
            RefreshSubcategoryList();
            RefreshSubcategoryList();
        }

        private async void AddCategoryBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NewCategoryTxt.Text)) return;

            Category newCategory = new Category();
            newCategory.Name = NewCategoryTxt.Text;

            await _categoriesRepository.InsertCategory(newCategory);
            await _categoriesCache.RefreshData();
            RefreshCategoryList();
            NewCategoryTxt.Text = "";
            SubcategoriesLbx.Visible = true;
        }

        private async void AddSubcategoryBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NewSubcategoryTxt.Text)) return;

            Subcategory newSubcategory = new Subcategory();
            newSubcategory.Name = NewSubcategoryTxt.Text;
            newSubcategory.CategoryId = _category.Id;

            if (_category.Id == 0)
            {
                MessageBox.Show("Please select category");
            }
            else
            {
                await _subcategoriesRepository.InsertSubcategory(newSubcategory);
                await _categoriesCache.RefreshData();
                RefreshSubcategoryList();
                NewSubcategoryTxt.Text = "";
                //Close();
            }
        }

        private void CategoriesLbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lbx = (ListBox)sender;
            _category = (Category)lbx.Items[lbx.SelectedIndex];
        }
    }
}
