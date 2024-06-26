﻿using FDMS.Model;
using FDMS.Repository.FlightRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Pilot_FlightController : ControllerBase
    {
        private readonly IFlightRepository _iFlightRepository;

        public Pilot_FlightController(IFlightRepository iFlightRepository)
        {
            this._iFlightRepository = iFlightRepository;
        }
        [HttpGet("get-flight")]
        [Authorize(Policy = "RequirePilot")]
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
        [Authorize(Policy = "RequirePilot")]
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
        [HttpPut("flight-confirm/{flightId}")]
        [Authorize(Policy = "RequirePilot")]
        public async Task<IActionResult> FlightConfirm(int flightId, FlightConfirmModel model)
        {
            try
            {
                return Ok(await _iFlightRepository.FlightConfirm(flightId,model));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
