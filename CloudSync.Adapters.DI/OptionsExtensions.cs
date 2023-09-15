using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudSync.Adapters.DI;

public static class OptionsExtensions
{
    public static T CreateAndAddOptions<T>(this IServiceCollection services, IConfiguration configuration, string sectionName)
        where T : class
    {
        var section = configuration.GetSection(sectionName);
        services.AddOptions().Configure<T>(section);
        return section.Get<T>();
    }
}
