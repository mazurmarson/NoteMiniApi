using NoteMiniApi.Context;
using NoteMiniApi.Models;

namespace NoteMiniApi.Services
{

    public interface INoteService
    {
       IEnumerable<Note> GetNotes();
       Note GetNoteById(int Id);
        void AddNote(Note note);
    }
    public class NoteService : INoteService
    {
        private readonly AppDbContext _context;
        public NoteService(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Note> GetNotes()
        {
            var Notes = _context.Notes.ToList();
            return Notes;
        }

        public Note GetNoteById(int Id)
        {
            var note = _context.Notes.Where(x => x.Id == Id).FirstOrDefault();
            return note;
        }

        public void AddNote(Note note)
        {
            note.CreateAt = DateTime.Now;
            _context.Notes.Add(note);
            _context.SaveChanges();
        }
    }
}