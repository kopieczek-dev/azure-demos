using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ManagedIdentityDemo.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfigurationSection _blobConfig;

    public string ImageBase64 { get; private set; }

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _blobConfig = configuration.GetSection("Blob");
    }

    public async Task OnGet()
    {
        // Connect to Azure Blob Storage using Access Key from Connection String
        // var blobServiceClient = new BlobServiceClient(_blobConfig["ConnectionString"]);
       
        // Connect to Azure Blob Storage using Managed Identity
        var blobServiceClient = new BlobServiceClient(
            new Uri(_blobConfig["AccountUrl"]), 
            new DefaultAzureCredential());
        
        var containerClient = 
            blobServiceClient.GetBlobContainerClient(_blobConfig["ContainerName"]);
        var blobClient = 
            containerClient.GetBlobClient(_blobConfig["BlobName"]);

        var response = await blobClient.DownloadContentAsync();
        var bytes = response.Value.Content.ToArray();
        ImageBase64 = Convert.ToBase64String(bytes);
    }
}