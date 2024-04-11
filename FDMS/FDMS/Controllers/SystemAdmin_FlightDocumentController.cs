using FDMS.Model;
using FDMS.Repository.FlightDocumentRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdmin_FlightDocumentController : ControllerBase
    {
        private readonly IFlightDocumentRepository _iFlightDocumentRepository;

        public SystemAdmin_FlightDocumentController(IFlightDocumentRepository flightDocumentRepository)
        {
            this._iFlightDocumentRepository = flightDocumentRepository;
        }
        [HttpGet("get-flight-document")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetDocument(string? docType, string? date, string? searchString)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.GetDocumentByGOStaff(docType, date, searchString));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight-document/{documentId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetDocumentById(int documentId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.GetDocumentById(documentId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight-document/{documentId}/update-version")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetDocumentUpdatedVersion(int documentId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.GetDocumentUpdatedVersion(documentId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight-id/{flightId}/get-flight-document")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetFlightDocumentByFlightId(int flightId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.GetDocumentByFlightId(flightId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("flight-id/{flightId}/create-flight-document")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreateFD(int flightId, [FromForm] FlightDocumentCreateModel model)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.CreateNew(flightId, model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete-flight-document/{documentId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> DeleteDocument(int documentId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.Delete(documentId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("flight-document/{documentId}/update")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> Update(int documentId, IFormFile file)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.Update(documentId, file));
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
