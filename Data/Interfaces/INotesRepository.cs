using Data.CustomQueryResults;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface INotesRepository
    {
        public event Action<string> OnError;
        public Task InsertNote(Note note);
        public Task<List<NoteWithCategories>> SelectNotes();
        public Task DeleteNote(int Id);
        public Task UpdateNote(Note note);
    }
}
