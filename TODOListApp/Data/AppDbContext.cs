using Microsoft.EntityFrameworkCore;
using TODOListApp.Models;

namespace TODOListApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }


    }
}