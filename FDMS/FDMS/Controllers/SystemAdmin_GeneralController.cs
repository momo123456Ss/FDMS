using FDMS.Model;
using FDMS.Repository.AccountRepository;
using FDMS.Repository.GeneralRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdmin_GeneralController : ControllerBase
    {
        private readonly IGeneralRepository _iGeneralRepository;

        public SystemAdmin_GeneralController(IGeneralRepository iGeneralRepository)
        {
            this._iGeneralRepository = iGeneralRepository;
        }
        [HttpPut("update-general")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> UpdateGeneral([FromForm] GeneralUpdateModel model)
        {
            try
            {
                return Ok(await _iGeneralRepository.EditGeneral(model));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
