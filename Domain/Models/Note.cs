using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string NoteText { get; set; } = string.Empty;
        public bool IsLearned { get; set; } = false;
        public int CategoryId { get; set; } = 0;
        public int SubcategoryId { get; set; } = 0;
        

        public Note(string title, int categoryId, int subcategoryId, string noteText, bool isLearned, int? id = null)
        {
            Title = title;
            NoteText = noteText;
            CategoryId = categoryId;
            SubcategoryId = subcategoryId;
            IsLearned = isLearned;
            if (id != null) 
                id = (int)id;
        }

        public Note() { }
    }
}
