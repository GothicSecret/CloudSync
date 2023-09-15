using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudSync.Adapters.DI;

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
        services.CreateAndAddOptions<ClientSecretOptions>(configuration, ClientSecretOptions.SectionName);
        return services;
    }
}
