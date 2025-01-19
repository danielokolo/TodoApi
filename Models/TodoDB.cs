using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Entity;

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<TodoItem> Todos => Set<TodoItem>();
}