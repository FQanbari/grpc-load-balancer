using System.Net.Http.Headers;
using System.Net;

namespace LoadBalancer;

public class BackendServer
{
    public string Id { get; }
    public string Endpoint { get; }
    public string HealthCheckEndpoint { get; }

    private readonly HttpClient httpClient;

    public BackendServer(string id, string endpoint, string healthCheckEndpoint)
    {
        Id = id;
        Endpoint = endpoint;
        HealthCheckEndpoint = healthCheckEndpoint;
        httpClient = new HttpClient();
    }

    public async Task<string> ProcessRequestAsync()
    {
        try
        {
            using (var client = HttpClient(Endpoint))
            {
                HttpResponseMessage response = await client.GetAsync(HealthCheckEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and return it
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Processed request on backend server {Id}. Response: {responseBody}");
                    return responseBody;
                }
                else
                {
                    // Handle the case when the backend server returns an error
                    Console.WriteLine($"Error processing request on backend server {Id}. Status Code: {response.StatusCode}");
                    return $"Error processing request on backend server {Id}. Status Code: {response.StatusCode}";
                }
            }
            
        }
        catch (Exception ex)
        {
            // Handle exceptions that may occur during the request
            Console.WriteLine($"Exception while processing request on backend server {Id}: {ex.Message}");
            return $"Exception while processing request on backend server {Id}: {ex.Message}";
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
    // Additional properties and methods related to backend servers can be added as needed
}
