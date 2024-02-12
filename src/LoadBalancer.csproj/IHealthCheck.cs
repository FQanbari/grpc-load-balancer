namespace LoadBalancer;

public interface IHealthCheck
{
    Task<List<BackendServer>> GetHealthyServersAsync(List<BackendServer> servers);
}