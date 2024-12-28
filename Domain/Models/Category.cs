using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Category(int id, string name) 
        {
            Id = id;
            Name = name;
        }

        public Category() { }
    }
}
