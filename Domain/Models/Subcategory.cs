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

        public Subcategory(int id, string name)
        {
            Name = name;
            Id = id;
        }

        public Subcategory() { }
    }
}
