using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApplicationInsightsDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(IConfiguration configuration, ILogger<DatabaseController> logger)
        {
            _configuration = configuration;
            this._logger = logger;
        }

        [HttpGet]
        public IActionResult GetCurrentNumberOfOrders()
        {
            int numberOfOrders = 0;

            var connectionString = 
                Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING")!;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "select count(*) from [SalesLT].[SalesOrderHeader]";
                SqlCommand command = new SqlCommand(query, connection);

                numberOfOrders = (int)command.ExecuteScalar();
            }

            return Ok(numberOfOrders);
        }
    }
}
