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

       
        [HttpGet("get-all-flight")]
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
        [HttpGet("get-flight-by-id/{id}")]
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
        [HttpGet("get-current-flight")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetCurrentFlight()
        {
            try
            {
                return Ok(await _iFlightRepository.GetCurrentFlight());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("create-new")]
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
        [HttpPost("flight-add-account")]
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
        [HttpDelete("flight-remove-account")]
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
