using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        public Subcategory(string name)
        {
            Name = name;
        }
        public Subcategory(int id, string name, int? categoryId = 0)
        {
            Name = name;
            Id = id;
            if (categoryId != null)
                CategoryId = (int)categoryId;
        }

        public Subcategory() { }
    }
}
