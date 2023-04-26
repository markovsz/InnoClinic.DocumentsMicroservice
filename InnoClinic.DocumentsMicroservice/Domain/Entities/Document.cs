using Azure;
using Azure.Data.Tables;

namespace Domain.Entities;

public class Document : ITableEntity
{
    public string FileName { get; set; }

    public string RowKey { get; set; }
    public string PartitionKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
