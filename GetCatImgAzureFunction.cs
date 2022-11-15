#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
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
