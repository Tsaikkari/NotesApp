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
        public string SubCategory {  get; set; } = string.Empty;
        public bool IsLearned { get; set; } = false;
        public int CategoryId { get; set; } = 0;
        public int SubcategoryId { get; set; } = 0;
    }
}
