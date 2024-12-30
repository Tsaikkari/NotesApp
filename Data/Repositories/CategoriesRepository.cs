using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Models;
using Dapper;
using Data.Interfaces;

namespace Data.Repositories
{
    public class CategoriesRepository: ICategoriesRepository
    {
        public event Action<string> OnError;
        private void ErrorOccured(string errorMessage)
        {
            OnError?.Invoke(errorMessage);
        }

        public async Task InsertCategory(Category category)
        {
            try
            {
                string query = @"INSERT INTO Categories 
                (Name) VALUES (@Name)";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    await connection.ExecuteAsync(query, category);
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while adding a category");
            }
        }

        public async Task<List<Category>> SelectCategories()
        {
            try
            {
                string query = @"SELECT * FROM Categories";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    return (await connection.QueryAsync<Category>(query)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while fetching categories.");
                return new List<Category>();
            }
        }
    }
}
