using FDMS.Model;
using FDMS.Repository.AccountRepository;
using FDMS.Repository.GeneralRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IGeneralRepository _iGeneralRepository;

        public GeneralController(IGeneralRepository iGeneralRepository)
        {
            _iGeneralRepository = iGeneralRepository;
        }
        [HttpGet("get-general")]
        public async Task<IActionResult> GetGeneral()
        {
            try
            {
                return Ok(await _iGeneralRepository.GetGeneral());
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
