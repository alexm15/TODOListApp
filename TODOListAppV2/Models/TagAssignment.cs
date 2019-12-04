namespace TODOListAppV2.Models
{
    public class TagAssignment
    {
        public int TodoId { get; set; }
        public string TagName { get; set; }
        public TodoItem TodoItem { get; set; }
        public Tag Tag { get; set; }
    }
}