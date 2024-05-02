namespace WebApplication1.Models.ViewModels
{
    public class ListViewModel
    {
        public List<TaskModel> TaskList { get; set; }

        public TaskModel Task { get; set; }

        public string ListName { get; set; }

        public int ListId { get; set; }

        public List<ListModel> Lists { get; set; }
    }
}