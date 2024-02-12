namespace LoadBalancer.Tests;

public class RoundRobinAlgorithmTests
{
    [Fact]
    public void SelectNextServer_ReturnsNextServerInOrder()
    {
        // Arrange
        var backendServers = new List<BackendServer>
        {
            new BackendServer("Server1", "http://192.168.1.153:80", "/health"),
            new BackendServer("Server2", "http://192.168.1.153:3000", "/health"),
            new BackendServer("Server3", "http://192.168.1.153:3001", "/health"),
        };

        var roundRobinAlgorithm = new RoundRobinAlgorithm();

        // Act & Assert
        Assert.Equal(backendServers[0], roundRobinAlgorithm.SelectNextServer(backendServers));
        Assert.Equal(backendServers[1], roundRobinAlgorithm.SelectNextServer(backendServers));
        Assert.Equal(backendServers[2], roundRobinAlgorithm.SelectNextServer(backendServers));

        // After reaching the end of the list, it should loop back to the beginning
        Assert.Equal(backendServers[0], roundRobinAlgorithm.SelectNextServer(backendServers));
    }

    [Fact]
    public void SelectNextServer_EmptyServerList_ReturnsNull()
    {
        // Arrange
        var roundRobinAlgorithm = new RoundRobinAlgorithm();

        // Act
        var selectedServer = roundRobinAlgorithm.SelectNextServer(new List<BackendServer>());

        // Assert
        Assert.Null(selectedServer);
    }

    // Add more test methods to cover other scenarios or edge cases
}