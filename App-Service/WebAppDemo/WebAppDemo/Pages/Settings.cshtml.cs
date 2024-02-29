using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppDemo.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly ILogger<SettingsModel> _logger;
        private readonly IConfiguration _configuration;

        public SettingsModel(ILogger<SettingsModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            var settingValue = _configuration["Demo:Setting"];
            ViewData["Setting"] = settingValue;

            _logger.Log(LogLevel.Debug, $"Read {settingValue} from config");
        }
    }

}
