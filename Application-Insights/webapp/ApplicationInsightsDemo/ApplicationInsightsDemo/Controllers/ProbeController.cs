using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApplicationInsightsDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProbeController : ControllerBase
    {
        [HttpGet("liveness")]
        public IActionResult Liveness()
        {
            return Ok();
        }

        [HttpGet("readiness")]
        public IActionResult Readiness()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            ExecuteDB();

            stopwatch.Stop();
            var executionTime = stopwatch.Elapsed;

            if (executionTime > TimeSpan.FromMilliseconds(10))
            {
                throw new Exception("Database query took too long");
            }

            return Ok();
        }

        private static void ExecuteDB()
        {
            var connectionString =
                Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING")!;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT @@VERSION";
                SqlCommand command = new SqlCommand(query, connection);

                var dbVer = (string)command.ExecuteScalar();
            }
        }
    }
}
