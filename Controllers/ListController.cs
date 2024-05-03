using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    public class ListController : Controller
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WebApplication1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private readonly WebApplication1Context _dbContext;

        public ListController(WebApplication1Context dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult RetrieveListItems(int listId)
        {
            List<TaskModel> tasks = RetrieveListItemsFromDatabase(listId);
            string listName = GetListName(listId);

            ListViewModel viewModel = new ListViewModel
            {
                TaskList = tasks,
                ListName = listName,
                ListId = listId
                // You can add other properties initialization here if needed
            };

            return View(viewModel);
        }

        private List<TaskModel> RetrieveListItemsFromDatabase(int listId)
        {
            List<TaskModel> taskList = new List<TaskModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT Id, Task, List_Id,IsCompleted FROM Tasks WHERE List_Id = '{listId}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taskList.Add(new TaskModel
                            {
                                Id = reader.GetInt32(0),
                                TaskName = reader.GetString(1),
                                ListId = reader.GetInt32(2),
                                IsComplete = reader.GetBoolean(3)
                            });
                        }
                    }
                }
            }

            return taskList;
        }

        private string GetListName(int listId)
        {
            string listName = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Name FROM Lists WHERE Id = @ListId";

                    command.Parameters.AddWithValue("@ListId", listId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            listName = reader.GetString(0);
                        }
                    }
                }
            }
            return listName;
        }


        [HttpPost]
        public IActionResult AddTask(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        // Prepare the SQL query
                        command.CommandText = "INSERT INTO Tasks (Task, List_Id, IsCompleted) VALUES (@Task, @ListId, @IsCompleted)";
                        command.Parameters.AddWithValue("@Task", task.TaskName);
                        command.Parameters.AddWithValue("@ListId", task.ListId);
                        command.Parameters.AddWithValue("@IsCompleted", 0);

                        // Execute the SQL command
                        command.ExecuteNonQuery();
                    }
                }

                // Return a JSON response indicating success
                return Json(new { success = true });
            }
            else
            {
                // If the model state is not valid, return an error message
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IActionResult DeleteTask(int id) // Accepting taskId as parameter
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (var deleteCmd = connection.CreateCommand())
                    {
                        connection.Open();
                        deleteCmd.CommandText = $"DELETE FROM Tasks WHERE Id = @id"; // Delete task with given Id
                        deleteCmd.Parameters.AddWithValue("@id", id); // Parameterized query to prevent SQL injection
                        deleteCmd.ExecuteNonQuery();
                    }
                }

                return Ok(); // Return OK if deletion is successful
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}