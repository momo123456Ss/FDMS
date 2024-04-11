using FDMS.Repository.FlightDocumentRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightDocumentController : ControllerBase
    {
        private readonly IFlightDocumentRepository _iFlightDocumentRepository;

        public FlightDocumentController(IFlightDocumentRepository flightDocumentRepository)
        {
            this._iFlightDocumentRepository = flightDocumentRepository;
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
    }
}
