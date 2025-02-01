using Microsoft.EntityFrameworkCore;
using ClassLibrary.Database.Models;

namespace ClassLibrary.Database;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Joke> Jokes { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
    {
        Database.EnsureCreated();
    }
}
