using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.Interfaces;

namespace Data.Repositories
{
    public class SubcategoriesRepository: ISubcategoriesRepository
    {
        public event Action<string> OnError;
        private void ErrorOccured(string errorMessage)
        {
            OnError?.Invoke(errorMessage);
        }

        public async Task InsertSubcategory(Subcategory subcategory)
        {
            try
            {
                string query = @"INSERT INTO Subcategories 
                (Name, CategoryId) VALUES (@Name, @CategoryId)";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    await connection.ExecuteAsync(query, subcategory);
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while adding a subcategory");
            }
        }

        public async Task<List<Subcategory>> SelectSubcategories()
        {
            try
            {
                string query = @"SELECT sc.Id, sc.Name, sc.CategoryId
                               FROM Subcategories AS sc
                               JOIN Categories AS c ON sc.CategoryId = c.Id";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    return (await connection.QueryAsync<Subcategory>(query)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while fetching subcategories.");
                return new List<Subcategory>();
            }
        }

        public async Task<List<Subcategory>> SelectGroupSubcategories(int CategoryId)
        {
            try
            {
                string query = @$"SELECT sc.Id, sc.Name, sc.CategoryId
                               FROM Subcategories AS sc
                               JOIN Categories AS c ON sc.CategoryId = c.Id
                               WHERE sc.CategoryId = {CategoryId}
                               GROUP BY sc.Id, sc.Name, CategoryId";

                using (IDbConnection connection = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    return (await connection.QueryAsync<Subcategory>(query)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorOccured("An error happened while fetching subcategories.");
                return new List<Subcategory>();
            }
        }
    }
}
