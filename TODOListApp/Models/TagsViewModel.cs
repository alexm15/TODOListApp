using System.Collections.Generic;

namespace TODOListApp.Models
{
    public class TagsViewModel
    {
        public Tag NewTag { get; set; }
        public IEnumerable<Tag> AvailableTags { get; set; }
    }
}