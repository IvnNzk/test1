using Microsoft.EntityFrameworkCore;
using WebApplication.models;

namespace WebApplication
{
    public class UploadDBContext : DbContext
    {
        public DbSet<Dataset> Datasets { get; set; }

        public UploadDBContext(DbContextOptions<UploadDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Должно передаватся через Конфиг ( при запуске локально ) или переменные окружения в докере
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
            optionsBuilder.UseNpgsql(
                @"host=localhost; port=3032; database=test_uploader_db;user id=user;password=pass;");
        } */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dataset>().ToTable("Dataset");
        }
    }
}