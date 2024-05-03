using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = WebApplication1; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent = ReadWrite; MultiSubnetFailover=False";

        private readonly TaskService _taskService;

        public HomeController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public IActionResult ToggleCompletion(int id, bool isComplete)
        {
            _taskService.UpdateTaskCompletion(id, isComplete);
            return Ok();
        }

        [HttpPost]
        public JsonResult DeleteList(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var tasksCmd = connection.CreateCommand())
                {
                    tasksCmd.CommandText = $"DELETE FROM Tasks WHERE List_Id = '{id}'";
                    tasksCmd.ExecuteNonQuery();
                }

                using (var listCmd = connection.CreateCommand())
                {
                    listCmd.CommandText = $"DELETE FROM Lists WHERE Id = '{id}'";
                    listCmd.ExecuteNonQuery();
                }
            }

            return Json(new { });
        }


        [Authorize]
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            HomeViewModel viewModel = new HomeViewModel
            {
                UserId = userId,
                Lists = RetrieveUserLists(userId)
            };
            return View(viewModel);
        }

        internal List<ListModel> RetrieveUserLists(string userName)
        {
            List<ListModel> lists = new List<ListModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var listCmd = connection.CreateCommand())
                {
                    connection.Open();
                    listCmd.CommandText = $"SELECT dbo.Lists.Id, dbo.Lists.Name FROM dbo.Lists JOIN dbo.AspNetUsers ON dbo.Lists.User_Id = dbo.AspNetUsers.Id WHERE dbo.AspNetUsers.Id = '{userName}'";

                    using (var reader = listCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lists.Add(
                                new ListModel
                                {
                                    ListId = reader.GetInt32(0),
                                    ListName = reader.GetString(1)
                                });
                        }
                    }
                }
            }
            return lists;
        }

        [HttpPost]
        public IActionResult AddList(ListModel list)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = "INSERT INTO Lists (Name, User_Id) VALUES (@Name, @UserId)";
                        command.Parameters.AddWithValue("@Name", list.ListName);
                        command.Parameters.AddWithValue("@UserId", list.UserId);

                        command.ExecuteNonQuery();
                    }
                }
                return Json(new { success = true });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}