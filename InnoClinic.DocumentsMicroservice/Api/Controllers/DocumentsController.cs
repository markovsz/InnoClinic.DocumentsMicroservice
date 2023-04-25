using Application.DTOs.Incoming;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsService _documentsService;

        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        [HttpPost("{partitionName}"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync(string partitionName, [FromBody] DocumentIncomingDto incomingDto)
        {
            string fileName = await _documentsService.UploadAsync(incomingDto, partitionName);
            return Ok(new { file = $"/api/Documents/{partitionName}/" + fileName});
        }


        [HttpGet("{partitionName}/{documentName}")]
        public async Task<IActionResult> DownloadAsync(string documentName, string partitionName)
        {
            var documentInfo = await _documentsService.DownloadAsync(documentName, partitionName);
            return File(documentInfo.Stream, documentInfo.ContentType);
        }

        [HttpDelete("{partitionName}/{documentName}")]
        public async Task DeleteAsync(string documentName, string partitionName)
        {
            await _documentsService.DeleteAsync(documentName, partitionName);
        }
    }
}
