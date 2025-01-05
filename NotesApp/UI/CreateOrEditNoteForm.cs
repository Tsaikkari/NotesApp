using Data.CustomQueryResults;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;
using DomainModel.Models;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.Categories;
using NotesApp.UI;

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

        private void ShowSuccessMessage(string successMessage)
        {
            MessageBox.Show(successMessage);
        }
        private void OnErrorOccured(string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }

        private void RefreshCategoryAndSubcategory(CategoryType categoryType)
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

        private void FilterCategories()
        {
            _catFilter = (Category)CategoryCbx.SelectedItem;

            CategoryCbx.DataSource = _categoriesCache.DefaultCategoryList;
            CategoryCbx.DisplayMember = "Name";

            FilterSubcategories();

            if (_catFilter != null && _catFilter.Id != 0)
            {
                int indexToSelect = FindCatIndex(_catFilter.Id);
                CategoryCbx.SelectedIndex = indexToSelect + 1;
            }
        }

        private void FilterSubcategories()
        {
            Subcategory subCatFilter = (Subcategory)SubcategoryCbx.SelectedItem;

            List<Subcategory> filterSubList = _categoriesCache.DefaultSubcategoryList;

            SubcategoryCbx.DataSource = filterSubList;
            SubcategoryCbx.DisplayMember = "Name";

            if (_catFilter != null && _catFilter.Id != 0)
            {
                if (subCatFilter != null && subCatFilter.Id != 0)
                {
                    FindSubcatIndex(subCatFilter.Id);
                }
            }
        }

        private void SetCategories()
        {
            if (_categoriesCache.CategoryNameFirstCatList != null && _categoriesCache.CategoryNameFirstSubcatList != null && CatName != null)
            { 
                FillFormForEdit();
                RefreshCategoryAndSubcategory(CategoryType.Selected);
            }
            else
                RefreshCategoryAndSubcategory(CategoryType.Default);
        }

        private int FindCatIndex(int categoryId)
        {
            List<Category> allCats = (List<Category>)CategoryCbx.DataSource;

            Category matchingCat = allCats.FirstOrDefault(c => c.Id == categoryId);

            int index = CategoryCbx.Items.IndexOf(matchingCat);
            return index;
        }

        private int FindSubcatIndex(int subcategoryId)
        {
            List<Subcategory> allSubcats = (List<Subcategory>)SubcategoryCbx.DataSource;

            Subcategory matchingSubcat = allSubcats.FirstOrDefault(c => c.Id == subcategoryId);

            int index = SubcategoryCbx.Items.IndexOf(matchingSubcat);
            return index;
        }

        private void FillFormForEdit()
        {
            FillForm(NoteId);
        }

        private async void FillForm(int? NoteId)
        {
            _notesCache = await _notesRepository.SelectNotes();
          
            NoteWithCategories clickedNote = _notesCache.FirstOrDefault(n => n.Id == NoteId);
         
            TitleTxtBox.Text = clickedNote.Title;
            NoteTxt.Text = clickedNote.NoteText;
        }

        private async void CreateNote_Load(object sender, EventArgs e)
        {
            await _categoriesCache.RefreshData();
            FilterCategories();
            SetCategories();
        }

        private void AddCategoryBtn_Click(object sender, EventArgs e)
        {
            CategoriesForm form = _serviceProvider.GetRequiredService<CategoriesForm>();
            form.ShowDialog();
            // TODO: should refresh right away the categories in the combo boxes
            form.FormClosed += async (sender, e) => await _categoriesCache.RefreshData();
        }

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

        private void ClearAllFields()
        {
            TitleTxtBox.Text = string.Empty;
            NoteTxt.Text = string.Empty;
            NoteId = 0;
        }
    }
}
