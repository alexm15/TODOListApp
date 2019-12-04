using System.Collections.Generic;

namespace TODOListAppV2.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public ICollection<TagAssignment> TagAssignments { get; set; } = new List<TagAssignment>();
    }
}