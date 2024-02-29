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
            ViewData["Setting"] = _configuration["Demo:Setting"];
        }
    }

}
