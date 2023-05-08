using Application.DTOs.Incoming;
using Application.DTOs.Outgoing;
using Application.Services.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Models;
using MimeTypes;

namespace Application.Services;

public class DocumentsService : IDocumentsService
{
    private readonly IBlobRepository _blobRepository;
    private readonly IDocumentsRepository _documentsRepository;

    public DocumentsService(IBlobRepository blobRepository, IDocumentsRepository documentsRepository)
    {
        _blobRepository = blobRepository;
        _documentsRepository = documentsRepository;
    }

    public async Task<DocumentCreatedOutgoingDto> UploadAsync(DocumentIncomingDto incomingDto, string partitionName)
    {
        var document = new Document();
        document.PartitionKey = partitionName;
        document.RowKey = Guid.NewGuid().ToString();
        var fileExtension = incomingDto.Name.Split('.').Last();
        document.FileName = document.RowKey + '.' + fileExtension;
        var stream = new MemoryStream(incomingDto.Base64Value);

        var contentType = MimeTypeMap.GetMimeType(fileExtension);

        var uri = await _blobRepository.UploadAsync(stream, document.FileName, contentType);
        await _documentsRepository.CreateAsync(document);
        var outgoingDto = new DocumentCreatedOutgoingDto()
        {
            FilePath = $"/api/Documents/{partitionName}/" + document.FileName
        };
        return outgoingDto;
    }

    public async Task<DocumentOutgoingDto> DownloadAsync(string documentName, string partitionName)
    {
        var documentId = documentName.Split('.').First();
        var document = await _documentsRepository.GetByIdAsync(documentId, partitionName);
        var documentInfo = await _blobRepository.DownloadAsync(document.FileName);
        var outgoingDto = new DocumentOutgoingDto();
        outgoingDto.Stream = documentInfo.Stream;
        outgoingDto.ContentType = documentInfo.ContentType;
        return outgoingDto;
    }

    public async Task DeleteAsync(string documentName, string partitionName)
    {
        var documentId = documentName.Split('.').First();
        await _documentsRepository.DeleteAsync(documentId, partitionName);
    }
}
