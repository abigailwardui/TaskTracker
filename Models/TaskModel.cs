namespace WebApplication1.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string TaskName { get; set; }
        public bool IsComplete { get; set; }
    }
}