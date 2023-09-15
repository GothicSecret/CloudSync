using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudSync.Adapters;

public static class CloudSyncAdapterExtensions
{
    public const string CloudSyncSectionName = "CloudSync";

    public static IServiceCollection AddCloudSyncAdapters(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCloudSyncOptions(configuration)
            .AddTransient<CloudStorageAdapter>()
            .AddTransient<StorageClientFactory>();
        return services;
    }

    public static IServiceCollection AddCloudSyncOptions(this IServiceCollection services, IConfiguration configuration)
    {
        //var options = configuration
        //    .GetRequiredSection(ClientSecretOptions.SectionName)
        //    .Get<ClientSecretOptions>();
        services.AddOptions().Configure<ClientSecretOptions>(GetClientSecretSection(configuration));
        return services;
    }

    private static IConfiguration GetClientSecretSection(this IConfiguration configuration)
    {
        return configuration.GetSection(ClientSecretOptions.SectionName);
    }
}
