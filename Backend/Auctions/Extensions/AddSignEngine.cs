using Auctions.Features.ImageModeration;
using Auctions.Features.ImageModeration.SignEngine;

namespace Auctions.Extensions;

public static class AddSignEngineExtension
{
    public static IServiceCollection AddAddSignEngine(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SignEngineOptions>(configuration.GetRequiredSection(SignEngineOptions.SectionName));
        services.AddHttpClient(SignEngine.HttpClientName,
            client => { client.BaseAddress = new Uri("https://api.sightengine.com"); });
        services.AddScoped<IImageModeration, SignEngine>();

        return services;
    }
}