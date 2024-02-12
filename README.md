# gRPC Load Balancer with Dynamic Service Discovery

This project implements a load-balancing service using gRPC to distribute incoming requests across multiple backend servers. It includes features such as dynamic service discovery, registration, round-robin or weighted load balancing algorithms, and health checks to monitor server availability.

## Features

- Dynamic service discovery and registration.
- Round-robin or weighted load balancing algorithms.
- Health checks to monitor server availability.

## Project Structure

- **LoadBalancerService:** Main service for load balancing.
- **ILoadBalancingAlgorithm:** Interface for load balancing algorithms.
- **RoundRobinAlgorithm:** Implementation of the round-robin load balancing algorithm.
- **IHealthCheck:** Interface for health checks.
- **HttpHealthCheck:** Implementation of health checks using HTTP requests.
- **IServiceDiscovery:** Interface for service discovery.
- **ConsulServiceDiscovery:** Implementation of service discovery using Consul.
- **IConfigurationProvider:** Interface for configuration providers.
- **FileConfigurationProvider:** Implementation of a configuration provider using files.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)

### Building and Running

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/gRPC-LoadBalancer.git
    cd gRPC-LoadBalancer
    ```

2. Build the project:

    ```bash
    dotnet build
    ```

3. **Build the Load Balancer Project:**
   - Build the load balancer project to generate the necessary DLL files.

     ```bash
     dotnet build LoadBalancer.csproj
     ```

4. **Reference Load Balancer DLL in Backend Project:**
   - Copy the generated DLL files (located in the `bin` or `bin\Debug` or `bin\Release` directory after the build) from the load balancer project to your backend project.

     ```bash
     dotnet add reference path\to\LoadBalancer.dll
     ```

5. **Update Backend Code:**
   - In your backend code, create an instance of the `LoadBalancerService` class and use it to process incoming requests.

     ```csharp
     IConfiguration configuration = new ConfigurationBuilder()
      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
      .AddJsonFile("appsettings.json")
      .Build();
     LoadBalancerConfig loadBalancerConfig = new LoadBalancerConfig();
     configuration.GetSection("LoadBalancer").Bind(loadBalancerConfig);
     var loadBalancer = new LoadBalancerService(loadBalancerConfig);
     var response = loadBalancer.ProcessRequestAsync().Result;
     // Handle the response as needed
     ```

### Usage

6. **Configure Load Balancer:**
   - Make sure to configure the load balancer by providing the necessary settings, such as backend server details and load balancing algorithm, in the `appsettings.json` file.

     ```json
     {
       "LoadBalancer": {
         "BackendServers": [
           {
             "Id": "Server1",
             "Endpoint": "http://localhost:5001",
             "HealthCheckEndpoint": "/health"
           },
           {
             "Id": "Server2",
             "Endpoint": "http://localhost:5002",
             "HealthCheckEndpoint": "/health"
           }
         ],
         "LoadBalancingAlgorithm": "RoundRobin",
         "HealthCheckIntervalSeconds": 10
       }
     }
     ```

7. **Run the Backend Project:**
   - Build and run your backend project.

     ```bash
     dotnet build
     dotnet run
     ```

By following these steps, your backend project should be able to utilize the load-balancing capabilities provided by the load balancer project. Ensure that the load balancer is properly configured and that the necessary dependencies are included in your backend project. Adjust the code and configuration based on your specific requirements and architecture.

## Configuration

The load balancer can be configured using the `appsettings.json` file. Customize backend server details, load balancing algorithms, and health check settings based on your project requirements.

## Contributing

Contributions are welcome! Please follow the [contribution guidelines](CONTRIBUTING.md).

## License

This project is licensed under the [MIT License](LICENSE).
