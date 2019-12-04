using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using TODOListAppV2.Models;

namespace TODOListAppV2.Data
{
    public static class DbInitializer
    {

        public static void Initialize(TodoContext context)
        {
            //context.Database.EnsureCreated();

            if (context.TodoItem.Any()) return;

            var todoItems = new[]
            {
                new TodoItem {Name = "Vacuum Cleaning", Description = "Clean first floor"},
                new TodoItem {Name = "Buy groceries", Description = "Local community store"},
                new TodoItem {Name = "Pay bills", Description = ""},
                new TodoItem {Name = "Clean up garage", Description = "Do it now!"},

            };

            context.AddRange(todoItems);
            context.SaveChanges();

            var tags = new[]
            {
                new Tag {Name = "House"},
                new Tag {Name = "Outdoor"},
                new Tag {Name = "Errands"},
                new Tag {Name = "Payments"},
            };
            context.AddRange(tags);
            context.SaveChanges();
        }
    }
}