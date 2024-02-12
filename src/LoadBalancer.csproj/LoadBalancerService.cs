namespace LoadBalancer;

public class LoadBalancerService
{
    private readonly List<BackendServer> backendServers;
    private readonly ILoadBalancingAlgorithm loadBalancingAlgorithm;
    private readonly IHealthCheck healthCheck;

    public LoadBalancerService(List<BackendServer> backendServers, ILoadBalancingAlgorithm loadBalancingAlgorithm, IHealthCheck healthCheck)
    {
        this.backendServers = backendServers ?? throw new ArgumentNullException(nameof(backendServers));
        this.loadBalancingAlgorithm = loadBalancingAlgorithm ?? throw new ArgumentNullException(nameof(loadBalancingAlgorithm));
        this.healthCheck = healthCheck ?? throw new ArgumentNullException(nameof(healthCheck));
    }

    public async Task<string> ProcessRequestAsync()
    {
        // Get the next healthy backend server using the load balancing algorithm
        var selectedServer = await GetNextHealthyServerAsync();

        if (selectedServer != null)
        {
            // Forward the request to the selected backend server
            string response = await selectedServer.ProcessRequestAsync();
            return response;
        }

        // Handle the case when no healthy server is available
        return "No healthy backend server available.";
    }

    private async Task<BackendServer> GetNextHealthyServerAsync()
    {
        // Filter out unhealthy servers
        var healthyServers = await healthCheck.GetHealthyServersAsync(backendServers);

        // Use the load balancing algorithm to select the next server
        var selectedServer = loadBalancingAlgorithm.SelectNextServer(healthyServers);

        return selectedServer;
    }
}
