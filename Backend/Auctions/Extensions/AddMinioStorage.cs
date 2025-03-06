using Auctions.Features.MinioStorage.MinioUpload;
using Minio;
using Minio.AspNetCore;
using MinioOptions = Auctions.Features.MinioStorage.Shared.Options.MinioOptions;

namespace Auctions.Extensions;

public static class AddMinioStorageExtension
{
    public static IServiceCollection AddMinioStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetRequiredSection(MinioOptions.SectionName));

        services.AddMinio(opts =>
        {
            var minioOptions = configuration
                .GetRequiredSection(MinioOptions.SectionName)
                .Get<MinioOptions>() ?? throw new ArgumentException(nameof(MinioOptions));
            opts.Endpoint = minioOptions.Endpoint;
            opts.SecretKey = minioOptions.SecretKey;
            opts.AccessKey = minioOptions.AccessKey;
            opts.ConfigureClient(client => client.WithSSL(false));
        });

        services.AddScoped<MinioUploader>();
        return services;
    }
}