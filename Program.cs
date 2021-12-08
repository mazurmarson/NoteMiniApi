using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NoteMiniApi.Context;
using NoteMiniApi.Models;
using NoteMiniApi.Requests;
using NoteMiniApi.Services;
using NoteMiniApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(NoteValidator));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

NoteRequests.RegisterEndpoints(app);

app.Run();

