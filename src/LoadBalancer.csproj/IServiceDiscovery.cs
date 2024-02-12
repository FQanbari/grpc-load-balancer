namespace LoadBalancer;

public interface IServiceDiscovery
{
    List<BackendServer> DiscoverServices();
}

