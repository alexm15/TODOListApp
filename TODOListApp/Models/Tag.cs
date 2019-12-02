using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TODOListApp.Models
{
    public class Tag
    {
        [Required]
        public string Name { get; set; }
        public ICollection<TodoTag> TodoTags { get; set; }
    }
}