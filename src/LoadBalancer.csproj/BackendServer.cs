using System.Net.Http.Headers;
using System.Net;

namespace LoadBalancer;

public class BackendServer
{
    public string Id { get; }
    public string Endpoint { get; }
    public string HealthCheckEndpoint { get; }

    private readonly HttpClient httpClient;
    private string ـresponse;

    public BackendServer(string id, string endpoint, string healthCheckEndpoint)
    {
        Id = id;
        Endpoint = endpoint;
        HealthCheckEndpoint = healthCheckEndpoint;
        httpClient = new HttpClient();
    }

    public async Task<string> ProcessRequestAsync()
    {
        return await Task.FromResult(ـresponse);
    }
    public async Task AddResponse(string response)
    {
        ـresponse = response ?? string.Empty;
    }
}
