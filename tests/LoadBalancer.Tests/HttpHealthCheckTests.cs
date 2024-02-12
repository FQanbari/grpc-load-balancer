using Moq;
using System.Net;

namespace LoadBalancer.Tests;

public class HttpHealthCheckTests
{
    [Fact]
    public async Task GetHealthyServersAsync_ReturnsHealthyServers()
    {
        // Arrange
        var httpClientWrapperMock = new Mock<IHttpClientWrapper>();

        // Set up HttpClientWrapper mock to return 200 OK for each health check
        httpClientWrapperMock
            .Setup(wrapper => wrapper.GetAsync(It.IsAny<Uri>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpHealthCheck = new HttpHealthCheck(httpClientWrapperMock.Object);

        var backendServers = new List<BackendServer>
        {
            new BackendServer("Server1", "http://192.168.1.153:80", "/health"),
            new BackendServer("Server2", "http://192.168.1.153:3000", "/health"),
            new BackendServer("Server3", "http://192.168.1.153:3001", "/health"), //healthy server
        };

        // Act
        var healthyServers = await httpHealthCheck.GetHealthyServersAsync(backendServers);

        // Assert
        Assert.Equal(1, healthyServers.Count);
        Assert.Contains(backendServers.Last(), healthyServers);
    }

    [Fact]
    public async Task GetHealthyServersAsync_IgnoresUnhealthyServers()
    {
        // Arrange
        var httpClientWrapperMock = new Mock<IHttpClientWrapper>();

        // Set up HttpClientWrapper mock to return 500 Internal Server Error for Server2
        httpClientWrapperMock
            .Setup(wrapper => wrapper.GetAsync(new Uri("http://192.168.1.153:80")))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        var httpHealthCheck = new HttpHealthCheck(httpClientWrapperMock.Object);

        var backendServers = new List<BackendServer>
        {
            new BackendServer("Server1", "http://192.168.1.153:80", "/health"),
            new BackendServer("Server2", "http://192.168.1.153:3000", "/health"),
            new BackendServer("Server3", "http://192.168.1.153:3001", "/health"), // healthy server
        };

        // Act
        var healthyServers = await httpHealthCheck.GetHealthyServersAsync(backendServers);

        // Assert
        Assert.Equal(1, healthyServers.Count);
        Assert.DoesNotContain(backendServers[1], healthyServers);
    }

    [Fact]
    public async Task GetHealthyServersAsync_HandlesException()
    {
        // Arrange
        var httpClientWrapperMock = new Mock<IHttpClientWrapper>();

        // Set up HttpClientWrapper mock to throw an exception for Server3
        httpClientWrapperMock
            .Setup(wrapper => wrapper.GetAsync(new Uri("http://192.168.1.153:80")))
            .ThrowsAsync(new HttpRequestException("Simulated exception"));

        var httpHealthCheck = new HttpHealthCheck(httpClientWrapperMock.Object);

        var backendServers = new List<BackendServer>
        {
            new BackendServer("Server1", "http://192.168.1.153:80", "/health"),
            new BackendServer("Server2", "http://192.168.1.153:3000", "/health"),
            new BackendServer("Server3", "http://192.168.1.153:3001", "/health"), //healthy server
        };

        // Act
        var healthyServers = await httpHealthCheck.GetHealthyServersAsync(backendServers);

        // Assert
        Assert.Equal(1, healthyServers.Count);
        Assert.DoesNotContain(backendServers[1], healthyServers);
    }

    // Add more test methods to cover other scenarios, error handling, etc.
}