using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Domain.Models;
using Domain.Abstractions;

namespace Infrastructure.Repositories;

public class BlobRepository : IBlobRepository
{
    protected readonly string _connectionString;
    protected readonly string _blobContainerName;

    public BlobRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AzureBlobStorageConnection");
        _blobContainerName = configuration.GetSection("AzureBlobStorage").GetSection("BlobContainer").Value;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var blob = await GetBlobClientAsync(_blobContainerName, fileName);
        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
        return blob.Uri.ToString();
    }

    public async Task<DocumentInfo> DownloadAsync(string fileName)
    {
        var blob = await GetBlobClientAsync(_blobContainerName, fileName);
        var fileStream = new MemoryStream();
        var response = await blob.DownloadToAsync(fileStream);
        var documentInfo = new DocumentInfo();
        fileStream.Position = 0;
        documentInfo.Stream = fileStream;
        documentInfo.ContentType = response.Headers.ContentType;
        return documentInfo;
    }

    private async Task<BlobClient> GetBlobClientAsync(string containerName, string fileName)
    {
        var container = new BlobContainerClient(_connectionString, containerName);
        var createResponse = await container.CreateIfNotExistsAsync();
        if (createResponse != null && createResponse.GetRawResponse().Status == 201)
            await container.SetAccessPolicyAsync(PublicAccessType.Blob);

        var blobClient = container.GetBlobClient(fileName);
        return blobClient;
    }
}
