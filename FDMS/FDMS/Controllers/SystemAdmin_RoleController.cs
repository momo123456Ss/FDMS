using FDMS.Model;
using FDMS.Repository.AccountRepository;
using FDMS.Repository.RoleRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdmin_RoleController : ControllerBase
    {
        private readonly IRoleRepository _iRoleRepository;

        public SystemAdmin_RoleController(IRoleRepository iRoleRepository)
        {
            this._iRoleRepository = iRoleRepository;
        }
        [HttpGet("get-all-role")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetAllAccount()
        {
            try
            {
                return Ok(await _iRoleRepository.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("update-role-by-id/{roleId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> UpdateRoleById(string roleId, RoleCreateOrUpdateModel model)
        {
            try
            {
                return Ok(await _iRoleRepository.Update(roleId,model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("create-new")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreateNew(RoleCreateOrUpdateModel model)
        {
            try
            {
                return Ok(await _iRoleRepository.CreateNew(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("role-delete/{roleId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> RoleRemove(string roleId)
        {
            try
            {
                return Ok(await _iRoleRepository.Delete(roleId));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
