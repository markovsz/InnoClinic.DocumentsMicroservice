using Application.Services;
using Application.Services.Abstractions;
using Application.Validators;
using Domain.Abstractions;
using Domain.RequestParameters;
using FluentValidation;
using Infrastructure.Repositories;
using InnoClinic.SharedModels.DTOs.Documents.Incoming;
using Microsoft.Extensions.Azure;

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

    public static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<DocumentIncomingDto>, DocumentIncomingDtoValidator>();
        services.AddScoped<IValidator<DocumentParameters>, DocumentParametersValidator>();
        services.AddScoped<IValidator<string>, PartitionNameValidator>();
    }
}
