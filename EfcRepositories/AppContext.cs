using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class AppContext : DbContext
{
    public DbSet<Post> Posts => Set<Post>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Post>()
    //         .HasOne(p => p.User)
    //         .WithMany(u => u.Posts)
    //         .HasForeignKey(p => p.UserId)
    //         .OnDelete(DeleteBehavior.Cascade);
    // }
}