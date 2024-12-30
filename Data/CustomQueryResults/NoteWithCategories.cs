using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.CustomQueryResults
{
    public class NoteWithCategories
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string NoteText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Subcategory {  get; set; } = string.Empty;
        public int LevelOfKnowledge { get; set; } 
        public int CategoryId { get; set; } = 0;
        public int SubcategoryId { get; set; } = 0;
    }
}
