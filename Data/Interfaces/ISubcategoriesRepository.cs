using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ISubcategoriesRepository
    {
        public event Action<string> OnError;
        public Task InsertSubcategory(Subcategory subcategory);
        public Task<List<Subcategory>> SelectSubcategories();
    }
}
