using Data.Interfaces;
using Domain.Models;

namespace NotesApp
{
    public partial class CreateOrEditNoteForm : Form
    {
        readonly INotesRepository _notesRepository;
        public CreateOrEditNoteForm(INotesRepository notesRepository)
        {
            InitializeComponent();
            _notesRepository = notesRepository;
        }

        private void CreateNote_Load(object sender, EventArgs e)
        {
            AddNoteBtn.Visible = true;
            EditNoteBtn.Visible = false;
        }

        private void AddCategoryBtn_Click(object sender, EventArgs e)
        {

        }

        private void AddSubcategoryBtn_Click(object sender, EventArgs e)
        {

        }

        private void AddNoteBtn_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;

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
