namespace LoadBalancer;

public class RoundRobinAlgorithm : ILoadBalancingAlgorithm
{
    private int currentIndex = -1;

    public BackendServer SelectNextServer(List<BackendServer> availableServers)
    {
        if (availableServers.Count == 0)
        {
            return null;
        }

        // Round-robin logic to select the next server
        currentIndex = (currentIndex + 1) % availableServers.Count;

        return availableServers[currentIndex];
    }
}