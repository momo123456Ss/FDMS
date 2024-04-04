using FDMS.Model;
using FDMS.Repository.AccountRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Owner_OwnerSettingsController : ControllerBase
    {
        private readonly IAccountRepository _iaccountRepository;

        public Owner_OwnerSettingsController(IAccountRepository iaccountRepository)
        {
            this._iaccountRepository = iaccountRepository;
        }
        [HttpPut("ChangeOwner/{email}")]
        [Authorize(Policy = "RequireOwner")]
        public async Task<IActionResult> ChangeOnwer(string email, string passwordConfim)
        {
            try
            {
                return Ok(await _iaccountRepository.ChangeOwner(email, passwordConfim));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetOwner")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetOwner()
        {
            try
            {
                return Ok(await _iaccountRepository.GetOwner());
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
