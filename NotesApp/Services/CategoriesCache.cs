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
        private readonly string _catName;
        private readonly string _subcatName;
    
        private List<Category> _categories;
        private List<Subcategory> _subcategories;
        private List<Subcategory> _groupSubcategories;

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
            

            GetDefaultCategories();
            GetDefaultSubcategories();
            GetSelectedCategories(_catName);
            GetSelectedSubcategories(_subcatName);
        }

        public void GetDefaultCategories()
        {
            DefaultCategoryList = new List<Category>();
            DefaultCategoryList.Add(new Category(0, "All notes"));
            DefaultCategoryList.AddRange(_categories);
        }

        public void GetDefaultSubcategories()
        {
            DefaultSubcategoryList = new List<Subcategory>();
            DefaultSubcategoryList.Add(new Subcategory(0, "All notes", 0));
            DefaultSubcategoryList.AddRange(_subcategories);
        }

        public List<Category> GetSelectedCategories(string? categoryName)
        {
            CategoryNameFirstCatList = new List<Category>();

            Category cat= _categories.FirstOrDefault(c => c.Name == categoryName);

            foreach (Category c in _categories)
            {
                if (c.Name != categoryName)
                {
                    CategoryNameFirstCatList.Add(c);
                }
            }
            CategoryNameFirstCatList.Insert(0, cat);
            CategoryNameFirstCatList.AddRange(_categories);
            return CategoryNameFirstCatList;
        }

        public List<Subcategory> GetSelectedSubcategories(string? categoryName)
        {
            CategoryNameFirstSubcatList = new List<Subcategory>();
         
            Subcategory subcat = _subcategories.FirstOrDefault(sc => sc.Name == categoryName);

            foreach (Subcategory sc in _subcategories)
            {
                if (!sc.Name.Equals(categoryName))
                {
                    CategoryNameFirstSubcatList.Add(sc);
                }
            }
            CategoryNameFirstSubcatList.Insert(0, subcat);
            CategoryNameFirstSubcatList.AddRange(_subcategories);
            return CategoryNameFirstSubcatList;
        }
    }
}
