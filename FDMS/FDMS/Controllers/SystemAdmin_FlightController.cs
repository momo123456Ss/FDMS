using FDMS.Model;
using FDMS.Repository.FlightRepository;
using FDMS.Repository.RoleRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdmin_FlightController : ControllerBase
    {
        private readonly IFlightRepository _iFlightRepository;

        public SystemAdmin_FlightController(IFlightRepository iFlightRepository)
        {
            this._iFlightRepository = iFlightRepository;
        }
        [HttpPost("CreateNew")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreateNew(FlightCreateModel model)
        {
            try
            {
                return Ok(await _iFlightRepository.CreateFlight(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllFlight")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetAllFlight(string? search, string? date)
        {
            try
            {
                return Ok(await _iFlightRepository.GetAll(search,date));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetFlightById/{id}")]
        [Authorize(Policy = "RequireAdministrator")]
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
        [HttpPost("FlightAddAccount")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> FlightAddAccount(FlightAddAccount model)
        {
            try
            {
                return Ok(await _iFlightRepository.AddAccount(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("FlightRemoveAccount")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> FlightRemoveAccount(FlightAddAccount model)
        {
            try
            {
                return Ok(await _iFlightRepository.RemoveAccount(model));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
