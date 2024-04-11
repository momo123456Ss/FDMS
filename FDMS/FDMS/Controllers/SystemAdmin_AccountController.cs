using FDMS.Model;
using FDMS.Repository.AccountRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdmin_AccountController : ControllerBase
    {
        private readonly IAccountRepository _iaccountRepository;

        public SystemAdmin_AccountController(IAccountRepository iaccountRepository)
        {
            this._iaccountRepository = iaccountRepository;
        }      
        [HttpPost("create-account")]
        [Authorize (Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreateModel model)
        {
            try
            {
                return Ok(await _iaccountRepository.CreateAccount(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("renew-password")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> RenewPassword([FromBody] MailData model)
        {
            try
            {
                return Ok(await _iaccountRepository.RenewPassword(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateAccount/{email}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> UpdateAccount(string email, [FromBody] AccountUpdateModel model)
        {
            try
            {
                return Ok(await _iaccountRepository.UpdateAccount(email,model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("get-account-by-email/{email}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetAccountByEmail(string email)
        {
            try
            {
                return Ok(await _iaccountRepository.GetByEmail(email));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("get-all-account")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetAllAccount(string? search)
        {
            try
            {
                return Ok(await _iaccountRepository.GetAll(search));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
