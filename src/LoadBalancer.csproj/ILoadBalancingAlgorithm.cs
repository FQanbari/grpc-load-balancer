namespace LoadBalancer;

public interface ILoadBalancingAlgorithm
{
    BackendServer SelectNextServer(List<BackendServer> availableServers);
}