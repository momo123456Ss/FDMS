using FDMS.Repository.AccountRepository;
using FDMS.Repository.SystemNoficationRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdmin_SystemNoficationController : ControllerBase
    {
        private readonly ISystemNoficationRepository _iSystemNoficationRepository;

        public SystemAdmin_SystemNoficationController(ISystemNoficationRepository iSystemNoficationRepository)
        {
            this._iSystemNoficationRepository = iSystemNoficationRepository;
        }
        [HttpGet("GetAllSystemNofication")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetAllSystemNofication(string? search, string? date)
        {
            try
            {
                return Ok(await _iSystemNoficationRepository.GetAll(search,date));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
