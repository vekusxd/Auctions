using Auctions.Features.MinioStorage.Shared.Options;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using System.Net.Mime;

namespace Auctions.Features.MinioStorage.MinioUpload;

public record MinioUploaderRequest(IFormFile File);

public record MinioUploaderResponse(string Url);

public class MinioUploader : Endpoint<MinioUploaderRequest, Results<Ok<MinioUploaderResponse>, BadRequest<string>>>
{
    private readonly IMinioClient _minioClient;
    private readonly MinioOptions _minioOptions;

    public MinioUploader(IMinioClient minioClient, IOptions<MinioOptions> minioOptions)
    {
        _minioOptions = minioOptions.Value;
        _minioClient = minioClient;
    }

    public override void Configure()
    {
        Post("/upload");
        AllowFileUploads();
    }

    public override async Task<Results<Ok<MinioUploaderResponse>, BadRequest<string>>> ExecuteAsync(
        MinioUploaderRequest req,
        CancellationToken ct)
    {
        const string bucketName = "uploads";
        var contentType = req.File.ContentType;
        if (contentType != MediaTypeNames.Image.Jpeg && contentType != MediaTypeNames.Image.Png &&
            contentType != MediaTypeNames.Image.Webp)
            return TypedResults.BadRequest("Unsupported image format");

        var objectName = $"{Guid.NewGuid()}.{Path.GetExtension(req.File.FileName)}";

        try
        {
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            var found = await _minioClient.BucketExistsAsync(beArgs, ct);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(mbArgs, ct);
            }

            using var fileStream = new MemoryStream();
            await req.File.CopyToAsync(fileStream, ct);
            var fileBytes = fileStream.ToArray();
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(new MemoryStream(fileBytes))
                .WithObjectSize(fileStream.Length)
                .WithContentType("application/octet-stream");
            await _minioClient.PutObjectAsync(putObjectArgs, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return TypedResults.BadRequest("Error uploading file");
        }

        return TypedResults.Ok(
            new MinioUploaderResponse($"http://{_minioOptions.Endpoint}/{bucketName}/{objectName}"));
    }
}