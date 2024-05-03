using System.Collections.Generic;
namespace WebApplication1.Models.ViewModels
{
    public class HomeViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public List<ListModel> Lists { get; set; }

        public int ListId { get; set; }
    }
}
