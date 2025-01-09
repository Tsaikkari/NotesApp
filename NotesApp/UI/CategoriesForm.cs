using Data.CustomQueryResults;
using Data.Interfaces;
using Data.Repositories;
using DomainModel.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
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
        private List<Category> _categories;
        private List<Subcategory> _subcategories;

        public CategoriesForm(ICategoriesRepository categoriesRepository, ISubcategoriesRepository subcategoriesRepository, IServiceProvider serviceProvider, INotesRepository notesRepository)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _categoriesRepository = categoriesRepository;
            _subcategoriesRepository = subcategoriesRepository;
            _categoriesCache = _serviceProvider.GetRequiredService<CategoriesCache>();

            StyleForm();
        }

        private async void RefreshCategoryList()
        {
            _categories = await _categoriesRepository.SelectCategories();
            await _categoriesCache.RefreshData();
            CategoriesLbx.DataSource = _categories;
            CategoriesLbx.DisplayMember = "Name";
        }

        private async void RefreshSubcategoryList()
        {
            _subcategories = await _subcategoriesRepository.SelectSubcategories();
            await _categoriesCache.RefreshData();
            SubcategoriesLbx.DataSource = _subcategories;
            SubcategoriesLbx.DisplayMember = "Name";
        }

        private void CategoriesForm_Load(object sender, EventArgs e)
        {
            RefreshCategoryList();
            RefreshSubcategoryList();
        }

        private async void AddCategoryBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NewCategoryTxt.Text)) return;

            Category newCategory = new Category();
            newCategory.Name = NewCategoryTxt.Text;

            await _categoriesRepository.InsertCategory(newCategory);
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

        private void StyleForm()
        {
            JObject styleConfig = ConfigManager.LoadStyleConfig();

            string primaryBg = (string)styleConfig["primaryBg"];
            string secondaryBg = (string)styleConfig["secondaryBg"];
            string primaryFg = (string)styleConfig["primaryFg"];
            string secondaryFg = (string)styleConfig["secondaryFg"];

            CategoriesLbx.BackColor = ColorTranslator.FromHtml(primaryBg);
            SubcategoriesLbx.BackColor = ColorTranslator.FromHtml(secondaryBg);


            AddCategoryBtn.BackColor = ColorTranslator.FromHtml((string)styleConfig["primaryBtnBg"]);
            AddCategoryBtn.ForeColor = ColorTranslator.FromHtml((string)styleConfig["primaryBtnFg"]);
            AddSubcategoryBtn.BackColor = ColorTranslator.FromHtml((string)styleConfig["tertiariBtnBg"]);
            AddSubcategoryBtn.ForeColor = ColorTranslator.FromHtml((string)styleConfig["tertiariBtnFg"]);
        }
    }
}
