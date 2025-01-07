using Data.CustomQueryResults;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;
using DomainModel.Models;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.Categories;
using NotesApp.UI;
using System.Windows.Forms;

namespace NotesApp
{
    public enum CategoryType { Default, Selected };

    public partial class CreateOrEditNoteForm : Form
    {
        private readonly INotesRepository _notesRepository;
        private readonly IServiceProvider _serviceProvider;

        private List<NoteWithCategories> _notesCache;
        private Category _catFilter;
        CategoriesCache _categoriesCache;

        public int NoteId { get; set; }
        public decimal LevelOfKnowledge { get; set; }
        public string CatName { get; set; }
        public string SubcatName { get; set; }

        public CreateOrEditNoteForm(INotesRepository notesRepository, IServiceProvider serviceProvider, ICategoriesRepository categoriesRepository, ISubcategoriesRepository subcategoriesRepository)
        {
            InitializeComponent();
            _notesRepository = notesRepository;
            _serviceProvider = serviceProvider;
            _categoriesCache = _serviceProvider.GetRequiredService<CategoriesCache>();

            _notesRepository.OnSuccess += ShowSuccessMessage;
            _notesRepository.OnError += OnErrorOccured;
        }

        //private void FilterSubcategories()
        //{
        //    Subcategory subCatFilter = (Subcategory)SubcategoryCbx.SelectedItem;

        //    List<Subcategory> filterSubList = _categoriesCache.DefaultSubcategoryList;

        //    SubcategoryCbx.DataSource = filterSubList;
        //    SubcategoryCbx.DisplayMember = "Name";

        //    if (_catFilter != null && _catFilter.Id != 0)
        //    {
        //        if (subCatFilter != null && subCatFilter.Id != 0)
        //        {
        //            int index = FindSubcatIndex(subCatFilter.Id);
        //            SubcategoryCbx.DataSource = index; 

        //            List<Subcategory> subCats = _categoriesCache.DefaultSubcategoryList;

        //            var catSubcats = subCats.GroupBy(sc => sc.CategoryId).FirstOrDefault(g => g.Key == _catFilter.Id).ToList();
        //            SubcategoryCbx.DataSource = catSubcats;
        //        } else
        //        {
        //            List<Subcategory> subCats = _categoriesCache.DefaultSubcategoryList;

        //            var catSubcats = subCats.GroupBy(sc => sc.CategoryId).FirstOrDefault(g => g.Key == _catFilter.Id).ToList();
        //            SubcategoryCbx.DataSource = catSubcats;
        //        }
        //    }
        //}

        private void ShowSuccessMessage(string successMessage)
        {
            MessageBox.Show(successMessage);
        }
        private void OnErrorOccured(string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }


        // Filters categories: sets and filters data in comboboxes 
        // Calls FindCatIndex and SeparateCatsWithSubcats
        private void FilterCategories()
        {
            List<Category> categories = _categoriesCache.DefaultCategoryList;
            // sets the initial data in category combobox
            CategoryCbx.DataSource = categories;
            CategoryCbx.DisplayMember = "Name";

            List<Subcategory> subcategories = _categoriesCache.DefaultSubcategoryList;
            // sets the initial data in subcategory combobox
            SubcategoryCbx.DataSource = subcategories;
            SubcategoryCbx.DisplayMember = "Name";

            // saves the selected category list item
            _catFilter = (Category)CategoryCbx.SelectedItem;

            // saves the selected subcategory list item
            Subcategory subCatFilter = (Subcategory)SubcategoryCbx.SelectedItem;

            // if user has selected a cat, set the item to the combobox
            if (_catFilter != null && _catFilter.Id != 0)
            {
                int indexToSelect = FindCatIndex(_catFilter.Id);
                CategoryCbx.SelectedIndex = indexToSelect + 1;
                // if user has also selected the subcat, set it to its combobox
                if (subCatFilter != null && subCatFilter.Id != 0)
                {
                    int index = FindSubcatIndex(subCatFilter.Id);
                    SubcategoryCbx.DataSource = index + 1;
                }
                // if user didn't select the subcat yet
                else
                {
                    SetSubcategoryList(_catFilter, subCatFilter);
                }
            }
        }

        // Finds the selected category's index and is called in FilterCategories
        private int FindCatIndex(int categoryId)
        {
            List<Category> allCats = (List<Category>)CategoryCbx.DataSource;

            Category matchingCat = allCats.FirstOrDefault(c => c.Id == categoryId);

            int index = CategoryCbx.Items.IndexOf(matchingCat);
            return index;
        }

        // Finds the selected subcategory's index and is called in FilterSubcategories
        private int FindSubcatIndex(int subcategoryId)
        {
            List<Subcategory> allSubcats = (List<Subcategory>)SubcategoryCbx.DataSource;

            Subcategory matchingSubcat = allSubcats.FirstOrDefault(c => c.Id == subcategoryId);

            int index = SubcategoryCbx.Items.IndexOf(matchingSubcat);
            return index;
        }

        // Called in FilterCategories. Separates cats that don't have subcats from
        // cats that do have them. Sets the data into the subcategory combobox
        private void SetSubcategoryList(Category _catFilter, Subcategory? subCatFilter)
        {
            int counter = 0;

            var groupSubcategories = _categoriesCache.DefaultSubcategoryList.GroupBy(sc => sc.CategoryId).ToList();

            foreach (var subcatGroup in groupSubcategories)
            {
                int catId = subcatGroup.Key;

                foreach (var sc in subcatGroup)
                {
                    if (_catFilter.Id == catId)
                    {
                        counter++;
                    }
                }
            }

            if (counter < 1)
            {
                SubcategoryCbx.DataSource = _categoriesCache.DefaultSubcategoryList;
                SubcategoryCbx.DisplayMember = "Name";
            }
            else
            {
                var selectedSubcats = _categoriesCache.DefaultSubcategoryList.GroupBy(sc => sc.CategoryId).FirstOrDefault(g => g.Key == _catFilter.Id).ToList();
                SubcategoryCbx.DataSource = selectedSubcats;
            }
        }


        // SETTING FORM DATA:
        // Fills the title and text part of the form for edit note. Called in FillFormForEdit
        private async void FillForm(int? NoteId)
        {
            _notesCache = await _notesRepository.SelectNotes();

            NoteWithCategories clickedNote = _notesCache.FirstOrDefault(n => n.Id == NoteId);

            TitleTxtBox.Text = clickedNote.Title;
            NoteTxt.Text = clickedNote.NoteText;
        }

        // Calls the FillForm
        private void FillFormForEdit()
        {
            FillForm(NoteId);
        }

        // Form validation, called when adding or editing note
        private bool isValid()
        {
            bool isValid = true;
            string message = "";

            if (string.IsNullOrEmpty(TitleTxtBox.Text))
            {
                isValid = false;
                message += "Please enter title.\n\n";
            }

            if (string.IsNullOrEmpty(NoteTxt.Text))
            {
                isValid = false;
                message += "Please enter text.\n\n";
            }

            if (CategoryCbx.Text == "All notes")
            {
                isValid = false;
                message += "Please select category.\n\n";
            }

            if (SubcategoryCbx.Text == "All notes")
            {
                isValid = false;
                message += "Please select subcategory.\n\n";
            }

            if (!isValid)
                MessageBox.Show(message, "Form not valid!");
            return isValid;
        }

        // Clears the form fields
        private void ClearAllFields()
        {
            TitleTxtBox.Text = string.Empty;
            NoteTxt.Text = string.Empty;
            NoteId = 0;
        }

        // Fills the lists in comboboxes with _categoryCache data, both for add and edit
        // Called in SetFormForEdit
        private void FillComboboxesWithCacheData(CategoryType categoryType)
        {
            List<Category> categoryList = new List<Category>();
            List<Subcategory> subcategoryList = new List<Subcategory>();

            if (categoryType == CategoryType.Default)
            {
                categoryList = _categoriesCache.DefaultCategoryList;
                CategoryCbx.DataSource = categoryList;

                subcategoryList = _categoriesCache.DefaultSubcategoryList;
                SubcategoryCbx.DataSource = subcategoryList;
            }
            else if (categoryType == CategoryType.Selected)
            {
                AddNoteBtn.Visible = false;
                EditNoteBtn.Visible = true;

                categoryList = _categoriesCache.GetSelectedCategories(CatName);
                CategoryCbx.DataSource = categoryList;

                subcategoryList = _categoriesCache.GetSelectedSubcategories(SubcatName);
                SubcategoryCbx.DataSource = subcategoryList;
            }
        }

        // Fills the form for edit note with cache data and is called in form load and when closing the CategoriesForm: RefreshCatCacheAndSetCats
        private void SetFormForEdit()
        {
            if (_categoriesCache.CategoryNameFirstCatList != null && _categoriesCache.CategoryNameFirstSubcatList != null && CatName != null)
            {
                // set form for edit
                FillFormForEdit();
                FillComboboxesWithCacheData(CategoryType.Selected);
            }
            else
                // set form for add
                FillComboboxesWithCacheData(CategoryType.Default);
        }


        // REFRESH CATS AND SUBCATS:
        // Refreshes both cats and subcats in the cache and calls SetFormForEdit
        // Is called when closing the CategoriesForm
        private async void RefreshAndSetDataForCatsAndSubcats()
        {
            await _categoriesCache.RefreshData();
            SetFormForEdit();
        }

        // Refresheds _categoriesCache, filters categories and fills the form for edit
        // on FORM LOAD **************************************************************
        private async void CreateNote_Load(object sender, EventArgs e)
        {
            await _categoriesCache.RefreshData();
            FilterCategories();
            SetFormForEdit();
        }


        // BUTTON CLICKS:
        // Creates and opens the CategoriesForm
        private void AddCategoryBtn_Click(object sender, EventArgs e)
        {
            CategoriesForm form = _serviceProvider.GetRequiredService<CategoriesForm>();
            form.FormClosed += (sender, e) => RefreshAndSetDataForCatsAndSubcats();
            form.ShowDialog();
        }

        // Creates a new note and clears the fields & closes the form
        private async void AddNoteBtn_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;

            int CategoryId = ((Category)CategoryCbx.SelectedItem).Id;
            int SubcategoryId = ((Subcategory)SubcategoryCbx.SelectedItem).Id;
            Note newNote = new Note(TitleTxtBox.Text, CategoryId, SubcategoryId, NoteTxt.Text);

            await _notesRepository.InsertNote(newNote);
            ClearAllFields();
            Close();
        }

        // Edits a note, clears the fields and closes the form
        private async void EditNoteBtn_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;

            int CategoryId = ((Category)CategoryCbx.SelectedItem).Id;
            int SubcategoryId = ((Subcategory)SubcategoryCbx.SelectedItem).Id;

            Note note = new Note(TitleTxtBox.Text, CategoryId, SubcategoryId, NoteTxt.Text, LevelOfKnowledge, NoteId);

            await _notesRepository.UpdateNote(note);
            ClearAllFields();
            Close();
        }


        // USER ACTIONS
        private void CreateOrEditNoteForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void CategoryCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterCategories();
        }

        private void SubcategoryCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterCategories();
        }
    }
}
