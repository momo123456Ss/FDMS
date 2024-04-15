using FDMS.Entity;
using FDMS.Model;
using FDMS.Repository.FlightDocumentRepository;
using FDMS.Service.CloudinaryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightDocumentController : ControllerBase
    {
        private readonly FDMSContext _context;
        private readonly IFlightDocumentRepository _iFlightDocumentRepository;
        private readonly ICloudinaryService _iCloudinaryService;


        public FlightDocumentController(IFlightDocumentRepository flightDocumentRepository, ICloudinaryService iCloudinaryService, FDMSContext context)
        {
            this._iFlightDocumentRepository = flightDocumentRepository;
            this._iCloudinaryService = iCloudinaryService;
            this._context = context;
        }
        private string GetContentType(string cloudinaryUrl)
        {
            var fileExtension = Path.GetExtension(cloudinaryUrl).ToLower();
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".mp4":
                    return "video/mp4";
                default:
                    return MediaTypeNames.Application.Octet;
            }
        }

        [HttpGet("flight-id/{flightId}/count-flight-document-cms")]
        public async Task<IActionResult> CountFlightDocumenyByFlightId_CMS(int flightId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.CountFlightDocumenyByFlightId_CMS(flightId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight-id/{flightId}/count-flight-document-mobile")]
        public async Task<IActionResult> CountFlightDocumenyByFlightId_Mobile(int flightId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.CountFlightDocumenyByFlightId_Mobile(flightId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight-document/{id}/download")]
        [Authorize]
        public async Task<IActionResult> FlightDocumentDownload(int id)
        {
            try
            {
                var response = await _iFlightDocumentRepository.GetDocumentById(id);
                var fileData = response.data as FlightDocumentViewModel;
                return File(await _iCloudinaryService.DownloadDocument(fileData.FileUrl)
                    , GetContentType(fileData.FileUrl), $"{fileData.FlightNo}-{fileData.FileName}-v{fileData.Version}.{fileData.VersionPatch}-{DateTime.Now:yyyy:MM:dd:HH:mm:ss}{fileData.FileType}");
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight/{flightId}/export-flight-document-as-zip")]
        [Authorize]
        public async Task<IActionResult> ExportDocumentAsZip(int flightId)
        {
            try
            {
                var Flight = await _context.Flights.FirstOrDefaultAsync(f => f.FlightId.Equals(flightId));
                return File(await _iCloudinaryService.DownloadFilesAsZip(await _iFlightDocumentRepository.GetListDocumentId(flightId)), "application/zip", $"Flight-document-{Flight.FlightCode}{Flight.FlightId.ToString("D3")}-{DateTime.Now:yyyy:MM:dd:HH:mm:ss}.zip");
            }
            catch 
            { 
                return BadRequest();
            }
        }
    }
}
