using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace ApplicationInsightsDemo.Filters
{
    public class CustomTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;

        public CustomTelemetryProcessor(ITelemetryProcessor next)
        {
            _next = next;
        }

        public void Process(ITelemetry item)
        {
            // https://learn.microsoft.com/en-us/azure/azure-monitor/app/api-filtering-sampling
            if (false) // check if its ok to send
            {
                return;
            }

            _next.Process(item);
        }
    }
}
