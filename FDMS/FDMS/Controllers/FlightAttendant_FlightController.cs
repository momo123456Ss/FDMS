using FDMS.Repository.FlightRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightAttendant_FlightController : ControllerBase
    {
        private readonly IFlightRepository _iFlightRepository;

        public FlightAttendant_FlightController(IFlightRepository iFlightRepository)
        {
            this._iFlightRepository = iFlightRepository;
        }
        [HttpGet("get-flight")]
        [Authorize(Policy = "RequireFlightAttendant")]
        public async Task<IActionResult> GetFlight()
        {
            try
            {
                return Ok(await _iFlightRepository.GetFlightAccount());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("get-flight-by-id/{id}")]
        [Authorize(Policy = "RequireFlightAttendant")]
        public async Task<IActionResult> GetFlight(int id)
        {
            try
            {
                return Ok(await _iFlightRepository.GetById(id));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
