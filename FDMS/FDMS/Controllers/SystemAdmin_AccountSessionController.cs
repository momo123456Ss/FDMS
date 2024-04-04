using FDMS.Repository.AccountRepository;
using FDMS.Repository.AccountSessionRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdmin_AccountSessionController : ControllerBase
    {
        private readonly IAccountSessionRepository _iAccountSessionRepository;

        public SystemAdmin_AccountSessionController(IAccountSessionRepository iAccountSessionRepository)
        {
            this._iAccountSessionRepository = iAccountSessionRepository;
        }
        [HttpGet("GetAllAccountInSession")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetAllAccountInSession()
        {
            try
            {
                return Ok(await _iAccountSessionRepository.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("Unlock")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> Unlock(List<string> emails)
        {
            try
            {
                return Ok(await _iAccountSessionRepository.Unlock(emails));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("Lock")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> Lock(List<string> emails)
        {
            try
            {
                return Ok(await _iAccountSessionRepository.Lock(emails));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
