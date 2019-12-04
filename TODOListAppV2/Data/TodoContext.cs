using Microsoft.EntityFrameworkCore;
using TODOListAppV2.Models;

namespace TODOListAppV2.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext (DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItem { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagAssignment> TagAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .HasKey(tag => tag.Name);

            modelBuilder.Entity<TagAssignment>()
                .HasKey(ta => new {ta.TodoId, ta.TagName});
        }
    }
}
