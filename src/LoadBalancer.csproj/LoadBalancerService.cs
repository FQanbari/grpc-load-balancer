using Consul;
using Microsoft.Extensions.Options;

namespace LoadBalancer;

public class LoadBalancerService
{
    private readonly List<BackendServer> backendServers;
    private readonly ILoadBalancingAlgorithm loadBalancingAlgorithm;
    private readonly IHealthCheck healthCheck;
    private readonly LoadBalancerConfig _loadBalancerConfig;
    private readonly LoadBalancingAlgorithmFactory algorithmFactory;

    public LoadBalancerService(LoadBalancerConfig loadBalancerConfig)
    {
        _loadBalancerConfig = loadBalancerConfig ?? throw new ArgumentNullException(nameof(loadBalancerConfig));
        this.backendServers = _loadBalancerConfig?.BackendServers ?? throw new ArgumentNullException(nameof(_loadBalancerConfig.BackendServers));
        algorithmFactory = new LoadBalancingAlgorithmFactory();
        this.loadBalancingAlgorithm = algorithmFactory.Create(_loadBalancerConfig.LoadBalancingAlgorithm);
        this.healthCheck = new HttpHealthCheck();

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



public class LoadBalancerConfig
{
    public List<BackendServer> BackendServers { get; set; }
    public string LoadBalancingAlgorithm { get; set; }
    public int HealthCheckIntervalSeconds { get; set; }
}
public interface ILoadBalancingAlgorithmFactory
{
    ILoadBalancingAlgorithm Create(string algorithmType);
}
public class LoadBalancingAlgorithmFactory : ILoadBalancingAlgorithmFactory
{
    public ILoadBalancingAlgorithm Create(string algorithmType)
    {
        switch (algorithmType.ToUpper())
        {
            case "ROUNDROBIN":
                return new RoundRobinAlgorithm();
            // Add cases for other algorithms as needed
            default:
                throw new ArgumentException($"Unsupported load balancing algorithm: {algorithmType}", nameof(algorithmType));
        }
    }
}
