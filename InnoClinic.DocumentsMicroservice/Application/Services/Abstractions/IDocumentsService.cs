using InnoClinic.SharedModels.DTOs.Documents.Incoming;
using InnoClinic.SharedModels.DTOs.Documents.Outgoing;

namespace Application.Services.Abstractions;

public interface IDocumentsService
{
    Task<DocumentCreatedOutgoingDto> UploadAsync(DocumentIncomingDto incomingDto, string partitionName);
    Task<DocumentOutgoingDto> DownloadAsync(string documentName, string partitionName);
    Task DeleteAsync(string documentName, string partitionName);
}
