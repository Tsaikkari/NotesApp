﻿using Data.CustomQueryResults;
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
using NotesApp.Categories;

namespace NotesApp.UI
{
    public partial class NotesForm : Form
    {
        private readonly INotesRepository _notesRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ISubcategoriesRepository _subcategoriesRepository;
        private readonly IServiceProvider _serviceProvider;
        

        private List<NoteWithCategories> _notesCache;
        CategoriesCache _categoriesCache;
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
            await _categoriesCache.RefreshData();
            FilterCategories();
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
                List<Subcategory> subCats = _categoriesCache.DefaultSubcategoryList;

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
            if (KnowledgeLevelNum.Value == 0)
                MessageBox.Show("Please enter knowledge level.\n\n");
            if (NoteTxt.Text == "")
                MessageBox.Show("Please select a note.\n\n");

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
