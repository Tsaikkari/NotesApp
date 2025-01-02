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
using System.Windows;
using Domain.Models;
using System.Drawing.Design;

namespace NotesApp.UI
{
    public partial class NotesForm : Form
    {
        private readonly INotesRepository _notesRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISubcategoriesRepository _subcategoriesRepository;

        private List<NoteWithCategories> _notesCache;
        private Category _catFilter;
        private bool _isOpen = false;

        internal static int noteToEditId;
        internal static decimal levelOfKnowledge;
        internal static string? categoryName;
        internal static string? subcategoryName;

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
            _catFilter = (Category)CategoryFilterCbx.SelectedItem;

            List<Category> cats = await _categoriesRepository.SelectCategories();

            List<Category> filterList = new List<Category>();
            filterList.Add(new Category(0, "All notes"));
            filterList.AddRange(cats);

            CategoryFilterCbx.DataSource = filterList;
            CategoryFilterCbx.DisplayMember = "Name";

            RefreshSubcategories();

            if (_catFilter != null && _catFilter.Id != 0)
            {
                int indexToSelect = FindCatIndex(_catFilter.Id);
                CategoryFilterCbx.SelectedIndex = indexToSelect + 1;
            }
        }

        private async void RefreshSubcategories()
        {
            Subcategory subCatFilter = (Subcategory)SubcategoryFilterCbx.SelectedItem;

            List<Subcategory> subCats = await _subcategoriesRepository.SelectSubcategories();

            List<Subcategory> filterSubList = new List<Subcategory>();
            filterSubList.Add(new Subcategory(0, "All notes", 0));
            filterSubList.AddRange(subCats);

            SubcategoryFilterCbx.DataSource = filterSubList;
            SubcategoryFilterCbx.DisplayMember = "Name";

            if (_catFilter != null && _catFilter.Id != 0)
            {
                if (subCatFilter != null && subCatFilter.Id != 0)
                {
                    FindSubcatIndex(subCatFilter.Id);
                }
            }
        }

        private void ClearNote()
        {
            TitleLbl.Text = "";
            NoteTxt.Text = "";
            KnowledgeLevelNum.Value = 0;
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

            DataGridViewColumn[] columns = new DataGridViewColumn[8];
            columns[0] = new DataGridViewTextBoxColumn() { DataPropertyName = "Id", Visible = false };
            columns[1] = new DataGridViewButtonColumn()
            {
                Text = "Title",
                Name = "TitleBtn",
                HeaderText = "Title",
                DataPropertyName = "Title"
            };
            columns[2] = new DataGridViewTextBoxColumn() { DataPropertyName = "NoteText", Visible = false };
            columns[3] = new DataGridViewTextBoxColumn() { DataPropertyName = "Category", HeaderText = "Category" };
            columns[4] = new DataGridViewTextBoxColumn() { DataPropertyName = "Subcategory", HeaderText = "Subcat" };
            columns[5] = new DataGridViewTextBoxColumn() { DataPropertyName = "LevelOfKnowledge", Visible = false };
            columns[6] = new DataGridViewButtonColumn()
            {
                Text = "Edit",
                Name = "EditBtn",
                HeaderText = "",
                UseColumnTextForButtonValue = true
            };
            columns[7] = new DataGridViewButtonColumn()
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

        private async void NotesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && NotesGrid.CurrentCell is DataGridViewButtonCell)
            {
                NoteWithCategories clickedNote = (NoteWithCategories)NotesGrid.Rows[e.RowIndex].DataBoundItem;

                if (NotesGrid.CurrentCell.OwningColumn.Name == "DeleteBtn")
                {
                    await _notesRepository.DeleteNote(clickedNote.Id);
                    RefreshNotesCache();
                    ClearNote();
                }
                else if (NotesGrid.CurrentCell.OwningColumn.Name == "EditBtn")
                {
                    noteToEditId = clickedNote.Id;
                    levelOfKnowledge = clickedNote.LevelOfKnowledge;
                    categoryName = CategoryFilterCbx.Text;
                    subcategoryName = SubcategoryFilterCbx.Text;
                   
                    CreateOrEditNoteForm form = _serviceProvider.GetRequiredService<CreateOrEditNoteForm>();
                    form.FormClosed += (sender, e) => RefreshNotesCache();
                    form.ShowDialog();
                }
                else if (NotesGrid.CurrentCell.OwningColumn.Name == "TitleBtn")
                {
                    TitleLbl.Text = clickedNote.Title;
                    NoteTxt.Text = clickedNote.NoteText;
                    KnowledgeLevelNum.Value = clickedNote.LevelOfKnowledge;
                    noteToEditId = clickedNote.Id;
                }
            }
        }

        private async void NotesForm_Load(object sender, EventArgs e)
        {
            CustomizeGridAppearance();
            await RefreshCategories();
            RefreshNotesCache();
        }

        private async void FilterGridData()
        {
            Category selectedCat = (Category)CategoryFilterCbx.SelectedItem;
            Subcategory selectedSubcat = (Subcategory)SubcategoryFilterCbx.SelectedItem;

            if (selectedCat.Id == 0)
                NotesGrid.DataSource = _notesCache;

            else if (selectedCat.Id != 0 && selectedSubcat.Id != 0)
            {
                NotesGrid.DataSource = _notesCache.Where(n => n.CategoryId == selectedCat.Id && n.SubcategoryId == selectedSubcat.Id).ToList();
            }
            else if (_isOpen)
            {
                List<Subcategory> subCats = await _subcategoriesRepository.SelectSubcategories();

                var catSubcats = subCats.GroupBy(sc => sc.CategoryId).FirstOrDefault(g => g.Key == selectedCat.Id).ToList();
                SubcategoryFilterCbx.DataSource = catSubcats;
            }
            else
                NotesGrid.DataSource = _notesCache.Where(n => n.CategoryId == selectedCat.Id).ToList();
        }

        private void NewNoteBtn_Click(object sender, EventArgs e)
        {
            noteToEditId = 0;
            CreateOrEditNoteForm form = _serviceProvider.GetService<CreateOrEditNoteForm>();
            form.FormClosed += (sender, e) => RefreshNotesCache();
            form.ShowDialog();
        }

        private void CategoryFilterCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSubcategories();
            FilterGridData();
            ClearNote();
            _isOpen = false;
        }

        private void SubcategoryFilterCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterGridData();
            ClearNote();
        }

        private void SubcategoryFilterCbx_DropDown(object sender, EventArgs e)
        {
            _isOpen = true;
            FilterGridData();
        }

        private async void EditKnowledgeLevelBtn_Click(object sender, EventArgs e)
        {
            if (noteToEditId != 0)
            {
                NoteWithCategories note = _notesCache.FirstOrDefault(n => n.Id == noteToEditId);

                Note noteToEdit = new Note(TitleLbl.Text, note.CategoryId, note.SubcategoryId, NoteTxt.Text, KnowledgeLevelNum.Value, noteToEditId);

                await _notesRepository.UpdateNote(noteToEdit);
                RefreshNotesCache();

                noteToEditId = 0;
            }
        }
    }
}
