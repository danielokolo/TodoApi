using Microsoft.EntityFrameworkCore;

public class WorldDbContext : DbContext
{
     public WorldDbContext(DbContextOptions<WorldDbContext> options) : base(options) { }
//    public DbSet<Country> Countries { get; set; }
 //   public DbSet<City> Cities { get; set; }
  //  public DbSet<Language> Languages { get; set; }
}