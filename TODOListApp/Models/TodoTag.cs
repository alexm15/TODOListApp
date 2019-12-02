namespace TODOListApp.Models
{
    public class TodoTag
    {
        public string TagName { get; set; }
        public Tag Tag { get; set; }
        public int TodoId { get; set; }
        public TodoItem TodoItem { get; set; }
    }
}