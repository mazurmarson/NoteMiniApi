using Microsoft.EntityFrameworkCore;
using NoteMiniApi.Models;

namespace NoteMiniApi.Context
{
    public class AppDbContext : DbContext
    {
        private string _connectionString = "Server=DESKTOP-A0EEVH8\\SQLEXPRESS;Database=NoteAPIDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public DbSet<Note> Notes {get; set;}      

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }   
    }
}