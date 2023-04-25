using Azure;
using Azure.Data.Tables;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class DocumentsRepository : IDocumentsRepository
{
    private readonly TableClient _tableClient;

	public DocumentsRepository(TableServiceClient serviceClient, IConfiguration configuration)
	{
		var documentsTable = configuration.GetSection("AzureTables:DocumentsTable").Value;
		_tableClient = serviceClient.GetTableClient(documentsTable);
	}

    public async Task CreateAsync(Document document)
    {
        var response = await _tableClient.UpsertEntityAsync(document);
        if (response.IsError)
            throw new InvalidOperationException();
    }

    public async Task<Document> GetByIdAsync(string id, string type)
    {
        var response = await _tableClient.GetEntityAsync<Document>(type, id);
        var document = response.Value;
        return document;
    }

    public async Task DeleteAsync(string id, string type)
    {
        var response = await _tableClient.DeleteEntityAsync(type, id);
        if (response.IsError)
            throw new InvalidOperationException();
    }
}
