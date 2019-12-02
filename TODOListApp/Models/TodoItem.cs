using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace TODOListApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayName("Tags")]
        public ICollection<TodoTag> TodoTags { get; set; } = new List<TodoTag>();

        public void AddTags(List<Tag> tags)
        {
            tags.ForEach(tag => TodoTags.Add(new TodoTag {Tag = tag, TagName = tag.Name, TodoItem = this, TodoId = Id}));
        }
        
    }
}
