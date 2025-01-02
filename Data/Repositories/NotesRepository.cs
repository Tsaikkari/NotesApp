using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.CustomQueryResults;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories
{
    public class NotesRepository: INotesRepository
    {
        public event Action<string> OnError;
        private void ErrorOccured(string errorMessage)
        {
            if (OnError != null)
                OnError.Invoke(errorMessage);
        }
        public async Task InsertNote(Note note)
        {
            try
            {
                string query = @"INSERT INTO Notes
                (Title, NoteText, CategoryId, SubcategoryId)
                VALUES (@Title, @NoteText, @CategoryId, @SubcategoryId)";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    await connection.ExecuteAsync(query, note);
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while adding note.");
            }
        }

        public async Task<List<NoteWithCategories>> SelectNotes()
        {
            try
            {
                string query = @"SELECT n.Id, n.Title, n.NoteText, n.LevelOfKnowledge, c.Name AS 'Category', sc.Name AS 'Subcategory', 
                               n.CategoryId, n.SubcategoryId
                               FROM Notes AS n 
                               JOIN Categories AS c ON n.CategoryId = c.Id
                               JOIN Subcategories AS sc ON n.SubcategoryId = sc.Id";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    return (await connection.QueryAsync<NoteWithCategories>(query)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while getting notes.");
                return new List<NoteWithCategories>();
            }
        }

        public async Task SelectNote(int Id)
        {
            try
            {
                string query = @$"SELECT Title, NoteText, LevelOfKnowledge FROM Notes WHERE Id={Id}";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    await connection.ExecuteAsync(query);
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while deleting note.");
            }
        }

        public async Task DeleteNote(int Id)
        {
            try
            {
                string query = @$"DELETE FROM Notes WHERE Id={Id}";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    await connection.ExecuteAsync(query);
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while deleting note.");
            }
        }
        public async Task UpdateNote(Note note)
        {
            try
            {
                string query = @"UPDATE Notes
                               SET
                               Title = @Title,
                               NoteText = @NoteText,
                               LevelOfKnowledge = @LevelOfKnowledge,
                               CategoryId = @CategoryId,
                               SubcategoryId = @SubcategoryId
                               WHERE Id = @Id";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    await connection.ExecuteAsync(query, note);
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while editing note.");
            }

        }
        public async Task<List<Note>> SelectAllNotes()
        {
            try
            {
                string query = "SELECT * FROM Notes";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    return (await connection.QueryAsync<Note>(query)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while fetching all notes.");
                return new List<Note>();
            }
        }
    }
}
