using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TodoApi.Models.Entity;

public class WorldDbContext : DbContext
{
     public WorldDbContext(DbContextOptions<WorldDbContext> options) : base(options) { }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<LanguageEntity> Languages { get; set; }
}