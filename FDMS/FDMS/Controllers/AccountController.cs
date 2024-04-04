using FDMS.Model;
using FDMS.Repository.AccountRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            try
            {
                return Ok(await _accountRepository.SignIn(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(AccountChangePasswordModel model)
        {
            try
            {
                return Ok(await _accountRepository.ChangePassword(model));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
