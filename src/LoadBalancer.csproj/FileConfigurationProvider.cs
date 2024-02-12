using Microsoft.Extensions.Configuration;

namespace LoadBalancer;

public class FileConfigurationProvider : IConfigurationProvider
{
    private readonly IConfiguration configuration;

    public FileConfigurationProvider(IConfiguration configuration)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string GetSetting(string key)
    {
        try
        {
            // Retrieve the setting value from the configuration
            // This example assumes the configuration is provided using Microsoft.Extensions.Configuration
            // You may use a different configuration mechanism based on your project requirements

            string settingValue = configuration[key];

            Console.WriteLine($"Retrieved setting: {key} = {settingValue}");

            return settingValue;
        }
        catch (Exception ex)
        {
            // Handle exceptions that may occur during configuration retrieval
            Console.WriteLine($"Exception while retrieving configuration setting {key}: {ex.Message}");
            return null;
        }
    }
}

