using NoteMiniApi.Context;
using NoteMiniApi.Models;

namespace NoteMiniApi.Services
{

    public interface INoteService
    {
       IEnumerable<Note> GetNotes();
       Note GetNoteById(int Id);
        void AddNote(Note note, string userId);
        void DeleteNote(Note note);

        void UpdateNote(Note note);
        IEnumerable<Note> GetUserNotes(string userId);
        bool isUserNote(string userId, int noteId);

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

        public void AddNote(Note note, string userId)
        {
            var user = _context.Users.Where(x => x.Id == Guid.Parse(userId)).FirstOrDefault();
            note.CreateAt = DateTime.Now;
            note.User = user;
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public void DeleteNote(Note note)
        {

            _context.Notes.Remove(note);
            _context.SaveChanges();
        }



        public void UpdateNote(Note note)
        {
         _context.Notes.Update(note);
            _context.SaveChanges();
        }

        public IEnumerable<Note> GetUserNotes(string userId)
        {
            var notes = _context.Notes.Where(x => x.UserId == Guid.Parse(userId)).ToList();
            return notes;
        }

        public bool isUserNote(string userId, int noteId)
        {
            return _context.Notes.Where(x => x.Id == noteId).Any(x => x.UserId == Guid.Parse(userId));
        }

    }
}