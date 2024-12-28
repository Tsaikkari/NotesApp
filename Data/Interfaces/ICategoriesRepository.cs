using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICategoriesRepository
    {
        public event Action<string> OnError;
        public Task InsertCategory(Category category);
        public Task<List<Category>> SelectCategories();

    }
}
