using System.Security.Claims;
using FluentValidation;
using System.Collections.Generic;
using NoteMiniApi.Models;
using NoteMiniApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            app.MapGet("/notes/mynotes", NoteRequests.GetUserNotes)
            .Produces<List<Note>>()
            .Produces(StatusCodes.Status404NotFound);


            app.MapPost("/notes", NoteRequests.CreateNote)
            .Produces<Note>(StatusCodes.Status201Created)
            .Accepts<Note>("application/json")
            .WithValidator<Note>();

            app.MapPut("/notes", NoteRequests.UpdateNote)
            .Produces<Note>(StatusCodes.Status201Created)
            .Produces<Note>(StatusCodes.Status404NotFound)
            .Accepts<Note>("application/json")
            .WithValidator<Note>();

            app.MapDelete("/notes/{id}", NoteRequests.DeleteNote)
            .Produces<Note>(StatusCodes.Status204NoContent)
            .Produces<Note>(StatusCodes.Status204NoContent)
            .Produces<Note>(StatusCodes.Status404NotFound)
            .ExcludeFromDescription();

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
        [Authorize]
        public static IResult CreateNote(INoteService service, Note note, ClaimsPrincipal User)
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier).Value);
            service.AddNote(note, userId);

           // return Results.Created($"/notes/{note.Id}", note);
           return Results.Ok();
        }
        [Authorize]
        public static IResult DeleteNote(INoteService service, int Id, ClaimsPrincipal User)
        {
            
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var note = service.GetNoteById(Id);
            if(note == null)
            {
                return Results.NotFound();
            }
            if(!service.isUserNote(userId, Id))
            {
                return Results.Forbid();
            }
            service.DeleteNote(note);
            return Results.NoContent();
        }
        [Authorize]
        public static IResult UpdateNote(INoteService service, Note note, ClaimsPrincipal User)
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(note == null)
            {
                return Results.NotFound();
            }
             if(!service.isUserNote(userId, note.Id))
            {
                return Results.Forbid();
            }
            note.CreateAt = System.DateTime.Now;
            service.UpdateNote(note);
            return Results.NoContent();
        }
        [Authorize]
        public static IResult GetUserNotes(INoteService service, ClaimsPrincipal User)
        {
            var userId = (User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var notes = service.GetUserNotes(userId);
            if(notes == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(notes);
        }
    }
}