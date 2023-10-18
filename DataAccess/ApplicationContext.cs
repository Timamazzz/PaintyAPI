using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Database.EnsureDeleted();   // delete old data base
        //Database.EnsureCreated();   // create new data base
    }
    
    public DbSet<User> Users { get; set; }
}


