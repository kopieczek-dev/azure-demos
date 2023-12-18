using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplicationInsigtsDemo
{
    public static class CatApi
    {
        [FunctionName("GetRandomCat")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var client = new HttpClient();

            var resp = await client.GetAsync("https://api.thecatapi.com/v1/images/search");
            var respString = await resp.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(respString);
            var catImgUrl = data[0].url;

            var imgResp = await client.GetAsync(catImgUrl.ToString());
            var img = await imgResp.Content.ReadAsStreamAsync();

            return new FileStreamResult(img, "image/jpeg");
        }
    }
}
