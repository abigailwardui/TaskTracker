using System.Data.SqlClient;

namespace WebApplication1.Services
{
    public class TaskService
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = Test; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent = ReadWrite; MultiSubnetFailover=False";

        public void UpdateTaskCompletion(int taskId, bool isComplete)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = "UPDATE ListItems SET COMPLETED = @IsComplete WHERE ITEM_ID = @TaskId";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IsComplete", isComplete);
                    command.Parameters.AddWithValue("@TaskId", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
