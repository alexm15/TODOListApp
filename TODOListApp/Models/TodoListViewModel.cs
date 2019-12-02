using System.Collections.Generic;

namespace TODOListApp.Models
{
    public class TodoListViewModel
    {
        public IEnumerable<TodoItem> TodoItems { get; set; }
        public TodoItem NewTodo { get; set; }
    }
}