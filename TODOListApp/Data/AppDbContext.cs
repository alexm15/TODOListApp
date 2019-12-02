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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TodoTag> TodoTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .HasKey(t => t.Name);

            modelBuilder.Entity<TodoTag>()
                .HasKey(tt => new { tt.TodoId, tt.TagName });
            modelBuilder.Entity<TodoTag>()
                .HasOne(tt => tt.TodoItem)
                .WithMany(todo => todo.TodoTags)
                .HasForeignKey(tt => tt.TodoId);
            modelBuilder.Entity<TodoTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TodoTags)
                .HasForeignKey(tt => tt.TagName);
        }
    }
}