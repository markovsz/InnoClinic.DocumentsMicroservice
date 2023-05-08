using Api.Extensions;
using Application.DTOs.Incoming;
using Application.Services.Abstractions;
using Domain.RequestParameters;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsService _documentsService;
        private readonly IValidator<DocumentIncomingDto> _documentIncomingDtoValidator;
        private readonly IValidator<DocumentParameters> _documentParametersValidator;
        private readonly IValidator<string> _partitionNameValidator;

        public DocumentsController(IDocumentsService documentsService, IValidator<DocumentIncomingDto> documentIncomingDtoValidator, IValidator<DocumentParameters> documentParametersValidator, IValidator<string> partitionNameValidator)
        {
            _documentsService = documentsService;
            _documentIncomingDtoValidator = documentIncomingDtoValidator;
            _documentParametersValidator = documentParametersValidator;
            _partitionNameValidator = partitionNameValidator;
        }

        [HttpPost("{partitionName}"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync(string partitionName, [FromBody] DocumentIncomingDto incomingDto)
        {
            var result = _documentIncomingDtoValidator.Validate(incomingDto);
            result.HandleValidationResult();
            result = _partitionNameValidator.Validate(partitionName);
            result.HandleValidationResult();
            var documentDto = await _documentsService.UploadAsync(incomingDto, partitionName);
            return Ok(documentDto);
        }


        [HttpGet("{partitionName}/{documentName}")]
        public async Task<IActionResult> DownloadAsync(string documentName, string partitionName)
        {
            var param = new DocumentParameters();
            param.PartitionName = partitionName;
            param.DocumentName = documentName;
            var result = _documentParametersValidator.Validate(param);
            result.HandleValidationResult();
            var documentInfo = await _documentsService.DownloadAsync(documentName, partitionName);
            return File(documentInfo.Stream, documentInfo.ContentType);
        }

        [HttpDelete("{partitionName}/{documentName}")]
        public async Task DeleteAsync(string documentName, string partitionName)
        {
            var param = new DocumentParameters();
            param.PartitionName = partitionName;
            param.DocumentName = documentName;
            var result = _documentParametersValidator.Validate(param);
            result.HandleValidationResult();
            await _documentsService.DeleteAsync(documentName, partitionName);
        }
    }
}
