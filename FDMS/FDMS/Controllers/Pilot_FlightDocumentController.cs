using FDMS.Repository.FlightDocumentRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Pilot_FlightDocumentController : ControllerBase
    {
        private readonly IFlightDocumentRepository _iFlightDocumentRepository;

        public Pilot_FlightDocumentController(IFlightDocumentRepository flightDocumentRepository)
        {
            this._iFlightDocumentRepository = flightDocumentRepository;
        }
        [HttpGet("flight/{flightId}/get-account-flight-update-document")]
        [Authorize(Policy = "RequirePilot")]
        public async Task<IActionResult> GetUpdateFlightDocumentByAccount_Mobile(int flightId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.GetUpdateFlightDocumentByAccount_Mobile(flightId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight-id/{flightId}/get-flight-document-original")]
        [Authorize(Policy = "RequirePilot")]
        public async Task<IActionResult> GetFlightDocumentByFlightId(int flightId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.GetOriginalDocumentByFlightId(flightId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight-id/{flightId}/get-flight-document-update")]
        [Authorize(Policy = "RequirePilot")]
        public async Task<IActionResult> GetUpdateDocumentByFlightId(int flightId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.GetUpdateDocumentByFlightId(flightId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight-id/{flightId}/get-flight-document-last-version")]
        [Authorize(Policy = "RequirePilot")]
        public async Task<IActionResult> GetLastVersionDocumentByFlightId(int flightId)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.GetLastVersionDocumentByFlightId(flightId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("flight-document/{documentId}")]
        [Authorize(Policy = "RequirePilot")]
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
        [HttpPut("flight-document/{documentId}/update")]
        [Authorize(Policy = "RequirePilot")]
        public async Task<IActionResult> Update(int documentId , IFormFile file)
        {
            try
            {
                return Ok(await _iFlightDocumentRepository.Update(documentId,file));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
