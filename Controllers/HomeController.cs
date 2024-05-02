using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = Test; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent = ReadWrite; MultiSubnetFailover=False";

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
        public JsonResult DeleteTask(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"DELETE from ListItems WHERE ITEM_ID = '{id}'";
                    tableCmd.ExecuteNonQuery();
                }
            }
            return Json(new { });
        }

        [HttpPost]
        public JsonResult DeleteList(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"DELETE from TaskLists WHERE LIST_ID = '{id}'; DELETE FROM ListItems WHERE LIST_ID = '{id}'";
                    tableCmd.ExecuteNonQuery();
                }
            }
            return Json(new { });
        }

        public IActionResult Index(int? id)
        {
            ListViewModel viewModel = new ListViewModel();

            viewModel.Lists = RetrieveUserLists();

            // Clear the ListName property when no list is selected
            viewModel.ListName = null;

            if (id.HasValue)
            {
                var selectedList = viewModel.Lists.FirstOrDefault(l => l.ListId == id.Value);

                if (selectedList != null)
                {
                    viewModel.ListName = selectedList.ListName;
                }

                viewModel.TaskList = RetrieveListItems(id.Value);
                viewModel.ListId = id.Value;
            }
            else
            {
                viewModel.TaskList = new List<TaskModel>();
            }

            return View(viewModel);
        }

        internal List<ListModel> RetrieveUserLists()
        {
            List<ListModel> lists = new();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var listCmd = connection.CreateCommand())
                {
                    connection.Open();
                    listCmd.CommandText = $"SELECT LIST_ID, USER_ID, LIST_NAME FROM dbo.TaskLists WHERE USER_ID = 1";

                    using (var reader = listCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lists.Add(
                                new ListModel
                                {
                                    ListId = reader.GetInt32(0),
                                    UserId = reader.GetInt32(1),
                                    ListName = reader.GetString(2)
                                });
                        }
                    }
                }
            }

            return lists;
        }

        internal List<TaskModel> RetrieveListItems(int listId)
        {
            List<TaskModel> taskList = new();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM ListItems WHERE LIST_ID = '{listId}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                taskList.Add(
                                    new TaskModel
                                    {
                                        Id = reader.GetInt32(0),
                                        ListId = reader.GetInt32(1),
                                        TaskName = reader.GetString(2),
                                        IsComplete = reader.GetBoolean(3)
                                    });
                            }
                        }
                        else
                        {
                            return taskList;
                        }
                    }
                }
            }
            return taskList;
        }


        public RedirectResult AddTask(TaskModel task, int listId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO ListItems (LIST_ID, ITEM_NAME, COMPLETED) VALUES (@ListId, @TaskName, 0)";
                    tableCmd.Parameters.AddWithValue("@ListId", listId);
                    tableCmd.Parameters.AddWithValue("@TaskName", task.TaskName);

                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return Redirect("/?id=" + listId);
        }


        public RedirectResult AddList(ListModel list)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO TaskLists (USER_ID, LIST_NAME) VALUES (1, '{list.ListName}')";

                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return Redirect("http://localhost:5161/");
        }

    }
}