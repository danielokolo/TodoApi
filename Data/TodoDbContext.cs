using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

    public class TodoDbContext : DbContext
    {
        public TodoDbContext (DbContextOptions<TodoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Todo> Todo { get; set; } = default!;
    }
