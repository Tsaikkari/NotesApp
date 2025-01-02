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
        public decimal LevelOfKnowledge { get; set; }
        public int CategoryId { get; set; } = 0;
        public int SubcategoryId { get; set; } = 0;
        

        public Note(string title, int categoryId, int subcategoryId, string noteText, decimal? levelOfKnowledge = null, int? id = null)
        {
            Title = title;
            NoteText = noteText;
            CategoryId = categoryId;
            SubcategoryId = subcategoryId;
            if (id != null) 
                Id = (int)id;
            if (levelOfKnowledge != null)
            {
                LevelOfKnowledge = (decimal)levelOfKnowledge;
            }
        }

        public Note() { }
    }
}
