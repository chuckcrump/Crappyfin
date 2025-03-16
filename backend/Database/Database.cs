using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class MovieDbContext : DbContext
{
    public DbSet<backend.Parser.MovieClass> Movies { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Movies.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<backend.Parser.MovieClass>().HasKey(m => m.Uuid);
    }
}