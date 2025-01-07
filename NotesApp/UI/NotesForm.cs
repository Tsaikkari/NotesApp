using Data.CustomQueryResults;
using Data.Interfaces;
using Data.Repositories;
using DomainModel.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
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
using NotesApp.Categories;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Metrics;

namespace NotesApp.UI
{
    
    public partial class NotesForm : Form
    {
        private readonly INotesRepository _notesRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubcategoriesRepository _subcategoriesRepository;
        private readonly IServiceProvider _serviceProvider;

        private List<NoteWithCategories> _notesCache;
        private readonly CategoriesCache _categoriesCache;
        private Category _catFilter;
        private bool _isOpen = false;
        private int noteToEditId;
      
        public NotesForm(INotesRepository notesRepository, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _notesRepository = notesRepository;
            _notesRepository.OnError += OnErrorOccured;
            _serviceProvider = serviceProvider;
            _categoriesCache = _serviceProvider.GetRequiredService<CategoriesCache>();
            _notesRepository.OnSuccess += ShowSuccessMessage;

            StyleForm();
        }

        private void ShowSuccessMessage(string successMessage)
        {
            MessageBox.Show(successMessage);
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

        private void FilterCategories()
        {
            _catFilter = (Category)CategoryFilterCbx.SelectedItem;

            CategoryFilterCbx.DataSource = _categoriesCache.DefaultCategoryList;
            CategoryFilterCbx.DisplayMember = "Name";

            FilterSubcategories();

            if (_catFilter != null && _catFilter.Id != 0)
            {
                int indexToSelect = FindCatIndex(_catFilter.Id);
                CategoryFilterCbx.SelectedIndex = indexToSelect + 1;
            }
        }

        private void FilterSubcategories()
        {
            Subcategory subCatFilter = (Subcategory)SubcategoryFilterCbx.SelectedItem;

            SubcategoryFilterCbx.DataSource = _categoriesCache.DefaultSubcategoryList;
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
            NoteTitleLbl.Text = "";
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
            JObject styleConfig = ConfigManager.LoadStyleDonfig();

            DataGridViewColumn[] columns = new DataGridViewColumn[8];

            columns[0] = new DataGridViewTextBoxColumn() { DataPropertyName = "Id", Visible = false };
            columns[1] = new DataGridViewButtonColumn()
            {
                Text = "Title",
                Name = "TitleBtn",
                HeaderText = "Title",
                DataPropertyName = "Title",
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = ColorTranslator.FromHtml((string)styleConfig["primaryBtnBg"]),
                    ForeColor = ColorTranslator.FromHtml((string)styleConfig["primaryBtnFg"]),

                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            };
            columns[2] = new DataGridViewTextBoxColumn() { DataPropertyName = "NoteText", Visible = false };
            columns[3] = new DataGridViewTextBoxColumn() { DataPropertyName = "Category", HeaderText = "Category" };
            columns[4] = new DataGridViewTextBoxColumn() { DataPropertyName = "Subcategory", HeaderText = "Subcategory" };
            columns[5] = new DataGridViewTextBoxColumn() { DataPropertyName = "LevelOfKnowledge", Visible = false };
            columns[6] = new DataGridViewButtonColumn()
            {
                Text = "Edit",
                Name = "EditBtn",
                HeaderText = "",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = ColorTranslator.FromHtml((string)styleConfig["tertiariBtnBg"]),
                    ForeColor = ColorTranslator.FromHtml((string)styleConfig["tertiariBtnFg"]),
                    Alignment = DataGridViewContentAlignment.MiddleCenter

                }
            };
            columns[7] = new DataGridViewButtonColumn()
            {
                Text = "Delete",
                Name = "DeleteBtn",
                HeaderText = "",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = ColorTranslator.FromHtml((string)styleConfig["secondaryBtnBg"]),
                    ForeColor = ColorTranslator.FromHtml((string)styleConfig["secondaryBtnFg"]),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
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
                    CategoryFilterCbx.DisplayMember = clickedNote.Category.ToString();
                    SubcategoryFilterCbx.DisplayMember = clickedNote.Subcategory.ToString();
                    CreateOrEditNoteForm form = _serviceProvider.GetRequiredService<CreateOrEditNoteForm>();
                    form.NoteId = clickedNote.Id;
                    form.LevelOfKnowledge = clickedNote.LevelOfKnowledge;
                    form.CatName = clickedNote.Category;
                    form.SubcatName = clickedNote.Subcategory;
                    form.FormClosed += (sender, e) => RefreshNotesCache();
                    form.ShowDialog();
                }
                else if (NotesGrid.CurrentCell.OwningColumn.Name == "TitleBtn")
                {
                    NoteTitleLbl.Text = clickedNote.Title;
                    NoteTxt.Text = clickedNote.NoteText;
                    KnowledgeLevelNum.Value = clickedNote.LevelOfKnowledge;
                    noteToEditId = clickedNote.Id;
                }
            }
        }

        private async void NotesForm_Load(object sender, EventArgs e)
        {
            CustomizeGridAppearance();
            await _categoriesCache.RefreshData();
            FilterCategories();
            FilterSubcategories();
            RefreshNotesCache();
        }

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
                SubcategoryFilterCbx.DataSource = _categoriesCache.DefaultSubcategoryList;
                SubcategoryFilterCbx.DisplayMember = "Name";
            }
            else
            {
                var selectedSubcats = _categoriesCache.DefaultSubcategoryList.GroupBy(sc => sc.CategoryId).FirstOrDefault(g => g.Key == _catFilter.Id).ToList();
                SubcategoryFilterCbx.DataSource = selectedSubcats;
            }
        }

        private void FilterGridData()
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
                List<Subcategory> subCats = _categoriesCache.DefaultSubcategoryList;

                if (subCats != null && subCats.Count > 0)
                {
                    SetSubcategoryList(selectedCat, selectedSubcat);
                }
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
            FilterSubcategories();
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
            if (NoteTxt.Text == "")
                MessageBox.Show("Please select a note.\n\n");

            if (KnowledgeLevelNum.Value == 0)
                MessageBox.Show("Please enter knowledge level.\n\n");
            
            if (noteToEditId != 0)
            {
                NoteWithCategories note = _notesCache.FirstOrDefault(n => n.Id == noteToEditId);

                Note noteToEdit = new Note(NoteTitleLbl.Text, note.CategoryId, note.SubcategoryId, NoteTxt.Text, KnowledgeLevelNum.Value, noteToEditId);

                await _notesRepository.UpdateNote(noteToEdit);
                RefreshNotesCache();

                noteToEditId = 0;
            }
        }

        private void StyleForm()
        {
            JObject styleConfig = ConfigManager.LoadStyleDonfig();

            string primaryBg = (string)styleConfig["primaryBg"];
            string secondaryBg = (string)styleConfig["secondaryBg"];
            string primaryFg = (string)styleConfig["primaryFg"];
            string secondaryFg = (string)styleConfig["secondaryFg"];

            LeftPanel.BackColor = ColorTranslator.FromHtml(primaryBg);
            RightPanel.BackColor = ColorTranslator.FromHtml(secondaryBg);
            NoteTitleLbl.BackColor = ColorTranslator.FromHtml(secondaryBg);
            NoteTitleLbl.ForeColor = ColorTranslator.FromHtml(primaryFg);

            NewNoteBtn.BackColor = ColorTranslator.FromHtml((string)styleConfig["primaryBtnBg"]);
            NewNoteBtn.ForeColor = ColorTranslator.FromHtml((string)styleConfig["primaryBtnFg"]);
            EditKnowledgeLevelBtn.BackColor = ColorTranslator.FromHtml((string)styleConfig["tertiariBtnBg"]);
            EditKnowledgeLevelBtn.ForeColor = ColorTranslator.FromHtml((string)styleConfig["tertiariBtnFg"]);
            NotesGrid.BackgroundColor = ColorTranslator.FromHtml(primaryBg);
            NotesGrid.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml(secondaryBg);
            NotesGrid.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml(primaryFg);
            NotesGrid.DefaultCellStyle.BackColor = ColorTranslator.FromHtml(primaryBg);
            NotesGrid.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml(primaryFg);
            NotesGrid.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
        }

        private void NotesGrid_RowsDefaultCellStyleChanged(object sender, EventArgs e)
        {

        }
    }
}
