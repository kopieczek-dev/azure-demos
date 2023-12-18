using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationInsightsDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomEventController : ControllerBase
    {
        private readonly ILogger<CustomEventController> _logger;
        private readonly TelemetryClient _telemetryClient;

        public CustomEventController(ILogger<CustomEventController> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("GET in CustomEventController");
            _telemetryClient.TrackEvent("CustomEventController.Get");
            return Ok();
        }
    }
}
