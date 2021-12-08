using NoteMiniApi.Models;
using NoteMiniApi.Services;

namespace NoteMiniApi.Requests
{
    public static class NoteRequests
    {
        public static WebApplication RegisterEndpoints(this WebApplication app)
        {
            app.MapGet("/notes", NoteRequests.GetAll)
            .Produces<List<Note>>();

            app.MapGet("/notes/{id}", NoteRequests.GetNoteById)
            .Produces<Note>()
            .Produces(StatusCodes.Status404NotFound);


            app.MapPost("/notes", NoteRequests.CreateNote)
            .Produces<Note>(StatusCodes.Status201Created)
            .Accepts<Note>("application/json");

            return app;
        }
        public static IResult GetAll(INoteService service)
        {
            var notes = service.GetNotes();

            return Results.Ok(notes);
        }

        public static IResult GetNoteById(INoteService service, int Id)
        {
            var note = service.GetNoteById(Id);
            if (note == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(note);
        }

        public static IResult CreateNote(INoteService service, Note note)
        {
            service.AddNote(note);

            return Results.Created($"/notes/{note.Id}", note);
        }
    }
}