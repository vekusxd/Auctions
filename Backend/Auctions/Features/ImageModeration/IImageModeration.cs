namespace Auctions.Features.ImageModeration;

public interface IImageModeration
{
    public Task<bool>CheckImage(MemoryStream image, string objectName, string contentType, CancellationToken ct = default);
}

