using System.Net.Http.Headers;
using System.Net;
using Consul;
using System.Net.Http.Json;

namespace LoadBalancer;
public class HttpHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;

    public HttpHealthCheck()
    {
        // You may want to use dependency injection to inject HttpClient
        _httpClient = new HttpClient();
        // Set a base address if all health check endpoints are relative URIs
        // _httpClient.BaseAddress = new Uri("http://base-address-here");
    }
    private readonly IHttpClientWrapper _httpClientWrapper;

    public HttpHealthCheck(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }

    public async Task<List<BackendServer>> GetHealthyServersAsync(List<BackendServer> servers)
    {
        var healthyServers = new List<BackendServer>();

        foreach (var server in servers)
        {
            if (await _IsServerHealthyAsync(server))
            {
                healthyServers.Add(server);
            }
        }

        Console.WriteLine("Healthy servers:");
        foreach (var server in healthyServers)
        {
            Console.WriteLine($"- {server.Id} at {server.Endpoint}");
        }

        return healthyServers;
    }

    private async Task<bool> _IsServerHealthyAsync(BackendServer server)
    {
        try
        {
            using (var client = HttpClient(server.Endpoint))
            {
                HttpResponseMessage response = await client.GetAsync(server.HealthCheckEndpoint);
                bool isHealthy = response.IsSuccessStatusCode;

                if (isHealthy)
                {
                    Console.WriteLine($"Health check passed for server {server.Id}. Status Code: {response.StatusCode}");
                }
                else
                {
                    Console.WriteLine($"Health check failed for server {server.Id}. Status Code: {response.StatusCode}");
                }

                return isHealthy;
            }
            
        }
        catch (Exception ex)
        {
            // An exception may be thrown if the server is unreachable or there is an issue with the health check
            Console.WriteLine($"Exception during health check for server {server.Id}: {ex.Message}");
            return false;
        }
    }
    private HttpClient HttpClient(string url, int Timeout = 5)
    {
        var client = new HttpClient { BaseAddress = new Uri(url) };
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        client.Timeout = TimeSpan.FromMinutes(Timeout);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }
}

public interface IHttpClientWrapper
{
    Task<HttpResponseMessage> GetAsync(Uri requestUri);
}

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> GetAsync(Uri requestUri)
    {
        return _httpClient.GetAsync(requestUri);
    }
}