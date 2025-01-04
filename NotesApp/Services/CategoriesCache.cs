using Data.Interfaces;
using Data.Repositories;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Categories
{
    public class CategoriesCache
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubcategoriesRepository _subcategoriesRepository;
        private string SelectedCategoryName { get; set; }

        private List<Category> _categories;
        private List<Subcategory> _subcategories;

        public List<Category> DefaultCategoryList;
        public List<Subcategory> DefaultSubcategoryList;

        public List<Category> CategoryNameFirstCatList;
        public List<Subcategory> CategoryNameFirstSubcatList;

        public CategoriesCache(ICategoriesRepository categoriesRepository, ISubcategoriesRepository subcategoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
            _subcategoriesRepository = subcategoriesRepository;
        }

        public async Task RefreshData()
        {
            _categories = await _categoriesRepository.SelectCategories();
            _subcategories = await _subcategoriesRepository.SelectSubcategories();

            ClassifyCategories(SelectedCategoryName);
        }

        public void ClassifyCategories(string? categoryName)
        {
            // default
            DefaultCategoryList = new List<Category>();
            DefaultSubcategoryList = new List<Subcategory>();

            DefaultCategoryList.Add(new Category(0, "All notes"));
            DefaultCategoryList.AddRange(_categories);
            DefaultSubcategoryList.Add(new Subcategory(0, "All notes", 0));
            DefaultSubcategoryList.AddRange(_subcategories);

            // selected (for edit note)
            CategoryNameFirstCatList = new List<Category>();
            CategoryNameFirstSubcatList = new List<Subcategory>();

            Category cat = _categories.FirstOrDefault(c => c.Name == categoryName);

            foreach (Category c in CategoryNameFirstCatList)
            {
                if (c.Name != categoryName)
                {
                    CategoryNameFirstCatList.Add(c);
                }
            }
            CategoryNameFirstCatList.Insert(0, cat);

            Subcategory subcat = _subcategories.FirstOrDefault(sc => sc.Name == categoryName);

            foreach (Subcategory sc in CategoryNameFirstSubcatList)
            {
                if (!sc.Name.Equals(categoryName))
                {
                    CategoryNameFirstSubcatList.Add(sc);
                }
            }
            CategoryNameFirstSubcatList.Insert(0, subcat);
        }
    }
}
