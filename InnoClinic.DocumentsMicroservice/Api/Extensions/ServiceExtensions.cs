using Application.Services;
using Application.Services.Abstractions;
using Domain.Abstractions;
using Infrastructure.Repositories;
using Microsoft.Extensions.Azure;
using System;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Core.Extensions;

namespace Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureAzureStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddBlobServiceClient(configuration.GetValue<string>("ConnectionStrings:AzureBlobStorageConnection") ??
                                               throw new NotImplementedException());
            clientBuilder.AddTableServiceClient(configuration.GetValue<string>("ConnectionStrings:AzureBlobStorageConnection") ??
                                                throw new NotImplementedException());
        });
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBlobRepository, BlobRepository>();
        services.AddScoped<IDocumentsRepository, DocumentsRepository>();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IDocumentsService, DocumentsService>();
    }
}
