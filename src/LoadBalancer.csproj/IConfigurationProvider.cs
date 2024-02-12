namespace LoadBalancer;

public interface IConfigurationProvider
{
    string GetSetting(string key);
}

