using Domain.Models;

namespace Domain.Abstractions;

public interface IBlobRepository
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task<DocumentInfo> DownloadAsync(string fileName);
}
