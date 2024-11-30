namespace GraphClientDemo;

using Microsoft.Graph;
using Azure.Identity;
using System.Threading.Tasks;

public class DemoGraphClient
{
    private readonly GraphServiceClient _graphClient;

    public DemoGraphClient(string clientId, string tenantId, string clientSecret)
    {
        var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        _graphClient = new GraphServiceClient(clientSecretCredential);
    }
    
    public async Task<string> GetUserName(string userId)
    {
        var user = await _graphClient.Users[userId]
            .GetAsync();

        return user?.DisplayName ?? string.Empty;
    }
}