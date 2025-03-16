using Microsoft.EntityFrameworkCore;

public class MovieDbContext : DbContext
{
    public DbSet<Parser.MovieClass> Movies { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Movies.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parser.MovieClass>().HasKey(m => m.Uuid);
    }
}
