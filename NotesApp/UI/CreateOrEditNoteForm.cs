using Data.CustomQueryResults;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;
using DomainModel.Models;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.UI;

namespace NotesApp
{
    public partial class CreateOrEditNoteForm : Form
    {
        private readonly INotesRepository _notesRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubcategoriesRepository _subcategoriesRepository;
        private readonly IServiceProvider _serviceProvider;

        private List<NoteWithCategories> _notesCache;
        private List<Category> _categoriesCache;
        private List<Subcategory> _subcategoriesCache;

        private int noteToEditId = NotesForm.noteToEditId;
        private string categoryName = NotesForm.categoryName;
        private string subcategoryName = NotesForm.subcategoryName;

        public CreateOrEditNoteForm(INotesRepository notesRepository, IServiceProvider serviceProvider, ICategoriesRepository categoriesRepository, ISubcategoriesRepository subcategoriesRepository)
        {
            InitializeComponent();
            _notesRepository = notesRepository;
            _serviceProvider = serviceProvider;
            _categoriesRepository = categoriesRepository;
            _subcategoriesRepository = subcategoriesRepository;
        }

        private async void RefreshCategories()
        {
            _categoriesCache = await _categoriesRepository.SelectCategories();
            _subcategoriesCache = await _subcategoriesRepository.SelectSubcategories();

            CategoryCbx.DataSource = _categoriesCache;
            CategoryCbx.DisplayMember = "Name";
            SubcategoryCbx.DataSource = _subcategoriesCache;
            SubcategoryCbx.DisplayMember = "Name";

            if (categoryName != "All notes" && subcategoryName != "All notes" && noteToEditId != 0)
                SetCategories();
            else
            {
                List<Category> filterList = new List<Category>();
                filterList.Add(new Category(0, "All notes"));
                filterList.AddRange(_categoriesCache);

                CategoryCbx.DataSource = filterList;
                CategoryCbx.DisplayMember = "Name";

                List<Subcategory> filterSubList = new List<Subcategory>();
                filterSubList.Add(new Subcategory(0, "All notes", 0));
                filterSubList.AddRange(_subcategoriesCache);

                SubcategoryCbx.DataSource = filterSubList;
                SubcategoryCbx.DisplayMember = "Name";
            }
        }

        private void SetCategories()
        {
            if (_categoriesCache != null && _subcategoriesCache != null)
            {
                List<Category> filterCatList = new List<Category>();
                Category cat = _categoriesCache.FirstOrDefault(c => c.Name == categoryName);

                foreach (Category c in filterCatList)
                {
                    if (c.Name != categoryName)
                        filterCatList.Add(c);
                }

                filterCatList.Insert(0, cat);
                CategoryCbx.DataSource = filterCatList;
                CategoryCbx.DisplayMember = "Name";

                List<Subcategory> filterSubcatList = new List<Subcategory>();
                Subcategory subcat = _subcategoriesCache.FirstOrDefault(c => c.Name == subcategoryName);

                foreach (Subcategory c in filterSubcatList)
                {
                    if (c.Name != subcategoryName)
                        filterSubcatList.Add(c);
                }

                filterSubcatList.Insert(0, subcat);
                SubcategoryCbx.DataSource = filterSubcatList;
                SubcategoryCbx.DisplayMember = "Name";
            }
        }

        private void FillFormForEdit()
        {
            if (noteToEditId != 0)
            {
                AddNoteBtn.Visible = false;
                FillForm(noteToEditId);
            }
            else
                AddNoteBtn.Visible = true;
        }

        private async void FillForm(int? noteToEditId)
        {
            _notesCache = await _notesRepository.SelectNotes();

            NoteWithCategories clickedNote = _notesCache.FirstOrDefault(n => n.Id == noteToEditId);

            TitleTxtBox.Text = clickedNote.Title;
            NoteTxt.Text = clickedNote.NoteText;
        }


        private void CreateNote_Load(object sender, EventArgs e)
        {
            FillFormForEdit();
            RefreshCategories();
        }

        private void AddCategoryBtn_Click(object sender, EventArgs e)
        {
            CategoriesForm form = _serviceProvider.GetRequiredService<CategoriesForm>();
            form.ShowDialog();
        }

        private async void AddNoteBtn_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;

            int CategoryId = ((Category)CategoryCbx.SelectedItem).Id;
            int SubcategoryId = ((Subcategory)SubcategoryCbx.SelectedItem).Id;
            Note noteToEdit = new Note(TitleTxtBox.Text, CategoryId, SubcategoryId, NoteTxt.Text);

            await _notesRepository.InsertNote(noteToEdit);
            ClearAllFields();
            AddNoteBtn.Visible = false;
            Close();
        }

        private async void EditNoteBtn_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;

            int CategoryId = ((Category)CategoryCbx.SelectedItem).Id;
            int SubcategoryId = ((Subcategory)SubcategoryCbx.SelectedItem).Id;
            decimal levelOfKnowledge = NotesForm.levelOfKnowledge;
            Note newNote = new Note(TitleTxtBox.Text, CategoryId, SubcategoryId, NoteTxt.Text, levelOfKnowledge, noteToEditId);

            await _notesRepository.UpdateNote(newNote);
            ClearAllFields();
            AddNoteBtn.Visible = true;
            EditNoteBtn.Visible = false;
            noteToEditId = 0;
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
            noteToEditId = 0;
            categoryName = string.Empty;
            subcategoryName = string.Empty;
        }
    }
}
