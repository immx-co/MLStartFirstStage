using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ThirdStage.Database;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
    {
        Database.EnsureCreated();
    }
}
