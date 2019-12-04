using System.Collections.Generic;

namespace TODOListAppV2.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<TagAssignment> TagAssignments { get; set; } = new List<TagAssignment>();
    }
}