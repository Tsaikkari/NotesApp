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

        public CreateOrEditNoteForm(INotesRepository notesRepository, IServiceProvider serviceProvider, ICategoriesRepository categoriesRepository, ISubcategoriesRepository subcategoriesRepository)
        {
            InitializeComponent();
            _notesRepository = notesRepository;
            _serviceProvider = serviceProvider;
            _categoriesRepository = categoriesRepository;
            _subcategoriesRepository = subcategoriesRepository;
        }

        private async void CreateNote_Load(object sender, EventArgs e)
        {
            AddNoteBtn.Visible = true;
            EditNoteBtn.Visible = false;
        }

        private void AddCategoryBtn_Click(object sender, EventArgs e)
        {
            CategoriesForm form = _serviceProvider.GetService<CategoriesForm>();
            form.ShowDialog();
        }

        private void AddSubcategoryBtn_Click(object sender, EventArgs e)
        {
            SubcategoriesForm form = _serviceProvider.GetService<SubcategoriesForm>();
            form.ShowDialog();
        }

        private async void AddNoteBtn_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;

            int CategoryId = ((Category)CategoryCbx.SelectedItem).Id;
            int SubcategoryId = ((Subcategory)SubcategoryCbx.SelectedItem).Id;
            Note newNote = new Note(TitleTxtBox.Text, CategoryId, SubcategoryId, NoteTxt.Text);

            await _notesRepository.InsertNote(newNote);
            await _notesRepository.SelectNotes();
            ClearAllFields();
        }

        private void EditNoteBtn_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;

            ClearAllFields();
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

            if (!isValid)
                MessageBox.Show(message, "Form not valid!");
            return isValid;
        }

        private void ClearAllFields()
        {
            TitleTxtBox.Text = string.Empty;
            NoteTxt.Text = string.Empty;
        }
    }
}
