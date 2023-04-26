using Domain.Entities;

namespace Domain.Abstractions;

public interface IDocumentsRepository
{
    Task CreateAsync(Document document);
    Task<Document> GetByIdAsync(string id, string type);
    Task DeleteAsync(string id, string type);
}
