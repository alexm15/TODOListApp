using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TODOListApp.Models
{
    public class TodoItemViewModel
    {
        public TodoItem TodoItem { get; set; }

        public IEnumerable<string> SelectedTags { get; set; }
        public List<SelectListItem> AvailableTags { get; set; } = new List<SelectListItem>();
    }
}