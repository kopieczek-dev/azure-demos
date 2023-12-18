using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApplicationInsightsDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatApiController : ControllerBase
    {
        private readonly ILogger<CatApiController> _logger;

        public CatApiController(ILogger<CatApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var client = new HttpClient();

            var resp = await client.GetAsync("https://api.thecatapi.com/v1/images/search");
            var respString = await resp.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(respString)
                ?? throw new ArgumentException(nameof(data));
            var catImgUrl = data[0].url;

            var imgResp = await client.GetAsync(catImgUrl.ToString());
            var img = await imgResp.Content.ReadAsStreamAsync();

            return new FileStreamResult(img, "image/jpeg");
        }
    }
}
