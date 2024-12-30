using Data.CustomQueryResults;
using Data.Interfaces;
using Data.Repositories;
using DomainModel.Models;
using Microsoft.Extensions.DependencyInjection;
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
    public partial class NotesForm : Form
    {
        private readonly INotesRepository _notesRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISubcategoriesRepository _subcategoriesRepository;

        internal int noteToEditId;

        private List<NoteWithCategories> _notesCache;
        public NotesForm(INotesRepository notesRepository, IServiceProvider serviceProvider, ICategoriesRepository categoriesRepository, ISubcategoriesRepository subcategoriesRepository)
        {
            InitializeComponent();
            _notesRepository = notesRepository;
            _notesRepository.OnError += OnErrorOccured;
            _serviceProvider = serviceProvider;
            _categoriesRepository = categoriesRepository;
            _subcategoriesRepository = subcategoriesRepository;
        }
        private void OnErrorOccured(string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }
        private async void RefreshNotesCache()
        {
            _notesCache = await _notesRepository.SelectNotes();
            FilterGridData();
        }

        private async Task RefreshCategories()
        {
            Category catFilter = (Category)CategoryFilterCbx.SelectedItem;
            Subcategory subCatFilter = (Subcategory)SubcategoryFilterCbx.SelectedItem;

            List<Category> cats = await _categoriesRepository.SelectCategories();
            List<Subcategory> subCats = await _subcategoriesRepository.SelectSubcategories();

            List<Category> filterList = new List<Category>();
            filterList.Add(new Category(0, "All categories"));
            filterList.AddRange(cats);

            CategoryFilterCbx.DataSource = filterList;
            CategoryFilterCbx.DisplayMember = "Name";

            List<Subcategory> filterSubList = new List<Subcategory>();
            filterSubList.Add(new Subcategory(0, "All subcategories", 0));
            filterSubList.AddRange(subCats);

            SubcategoryFilterCbx.DataSource = filterSubList;
            SubcategoryFilterCbx.DisplayMember = "Name";

            if (catFilter != null && catFilter.Id != 0)
            {
                int indexToSelect = FindCatIndex(catFilter.Id);
                CategoryFilterCbx.SelectedIndex = indexToSelect + 1;

                if (subCatFilter != null && subCatFilter.Id != 0)
                {
                    FindSubcatIndex(subCatFilter.Id);
                }
            }
        }

        private int FindCatIndex(int categoryId)
        {
            List<Category> allCats = (List<Category>)CategoryFilterCbx.DataSource;

            Category matchingCat = allCats.FirstOrDefault(c => c.Id == categoryId);

            int index = CategoryFilterCbx.Items.IndexOf(matchingCat);
            return index;
        }

        private int FindSubcatIndex(int subcategoryId)
        {
            List<Subcategory> allSubcats = (List<Subcategory>)SubcategoryFilterCbx.DataSource;

            Subcategory matchingSubcat = allSubcats.FirstOrDefault(c => c.Id == subcategoryId);

            int index = SubcategoryFilterCbx.Items.IndexOf(matchingSubcat);
            return index;
        }

        private void CustomizeGridAppearance()
        {
            NotesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            NotesGrid.AutoGenerateColumns = false;

            DataGridViewColumn[] columns = new DataGridViewColumn[7];
            columns[0] = new DataGridViewTextBoxColumn() { DataPropertyName = "Id", Visible = false };
            columns[1] = new DataGridViewTextBoxColumn() { DataPropertyName = "Title", HeaderText = "Title" };
            columns[2] = new DataGridViewTextBoxColumn() { DataPropertyName = "NoteText", Visible = false };
            columns[3] = new DataGridViewTextBoxColumn() { DataPropertyName = "Category", HeaderText = "Category" };
            columns[4] = new DataGridViewTextBoxColumn() { DataPropertyName = "Subcategory", HeaderText = "Subcat" };
            columns[5] = new DataGridViewTextBoxColumn() { DataPropertyName = "LevelOfKnowledge", Visible = false };
            columns[5] = new DataGridViewButtonColumn()
            {
                Text = "Edit",
                Name = "EditBtn",
                HeaderText = "",
                UseColumnTextForButtonValue = true
            };
            columns[6] = new DataGridViewButtonColumn()
            {
                Text = "Delete",
                Name = "DeleteBtn",
                HeaderText = "",
                UseColumnTextForButtonValue = true
            };


            NotesGrid.RowHeadersVisible = false;
            NotesGrid.Columns.Clear();
            NotesGrid.Columns.AddRange(columns);
        }

        private async void NotesForm_Load(object sender, EventArgs e)
        {
            CustomizeGridAppearance();
            await RefreshCategories();
            RefreshNotesCache();
        }

        private void FilterGridData()
        {
            Category selectedCat = (Category)CategoryFilterCbx.SelectedItem;
            Subcategory selectedSubcat = (Subcategory)SubcategoryFilterCbx.SelectedItem;

            if (selectedCat.Id == 0)
            {
                NotesGrid.DataSource = _notesCache;
            }
            else if (selectedCat.Id != 0 && selectedSubcat.Id != 0)
            {
                NotesGrid.DataSource = _notesCache.Where(n => n.CategoryId == selectedCat.Id && n.SubcategoryId == selectedSubcat.Id).ToList();
            }
            else
            {
                NotesGrid.DataSource = _notesCache.Where(n => n.CategoryId == selectedCat.Id).ToList();
            }
        }

        private void NewNoteBtn_Click(object sender, EventArgs e)
        {
            CreateOrEditNoteForm form = _serviceProvider.GetService<CreateOrEditNoteForm>();
            form.FormClosed += (sender, e) => RefreshNotesCache();
            form.ShowDialog();
        }

        private async void CategoryFilterCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterGridData();
        }

        private void SubcategoryFilterCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterGridData();
        }

        private async void NotesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && NotesGrid.CurrentCell is DataGridViewButtonCell)
            {
                NoteWithCategories clickedNote = (NoteWithCategories)NotesGrid.Rows[e.RowIndex].DataBoundItem;

                if (NotesGrid.CurrentCell.OwningColumn.Name == "DeleteBtn")
                {
                    await _notesRepository.DeleteNote(clickedNote.Id);
                    RefreshNotesCache();
                }
                else if (NotesGrid.CurrentCell.OwningColumn.Name == "EditBtn")
                {
                    CreateOrEditNoteForm form = _serviceProvider.GetRequiredService<CreateOrEditNoteForm>();
                    form.FormClosed += (sender, e) => RefreshNotesCache();
                    form.ShowDialog();
                    noteToEditId = clickedNote.Id;
                }
            }
        }
    }
}
