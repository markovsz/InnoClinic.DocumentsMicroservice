using Application.DTOs.Incoming;
using Application.DTOs.Outgoing;

namespace Application.Services.Abstractions;

public interface IDocumentsService
{
    Task<DocumentCreatedOutgoingDto> UploadAsync(DocumentIncomingDto incomingDto, string partitionName);
    Task<DocumentOutgoingDto> DownloadAsync(string documentName, string partitionName);
    Task DeleteAsync(string documentName, string partitionName);
}
