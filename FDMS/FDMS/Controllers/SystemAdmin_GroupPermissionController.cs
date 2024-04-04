using FDMS.Model;
using FDMS.Repository.GroupPermission;
using FDMS.Repository.RoleRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdmin_GroupPermissionController : ControllerBase
    {
        private readonly IGroupPermissionRepository _iGroupPermissionRepository;

        public SystemAdmin_GroupPermissionController(IGroupPermissionRepository iGroupPermissionRepository)
        {
            this._iGroupPermissionRepository = iGroupPermissionRepository;
        }
        [HttpGet("GetAllGroup")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetAllGroup()
        {
            try
            {
                return Ok(await _iGroupPermissionRepository.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetGroupById/{groupId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetGroupById(int groupId)
        {
            try
            {
                return Ok(await _iGroupPermissionRepository.GetById(groupId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateGroupById/{groupId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> UpdateRoleById(int groupId, GroupPermissionCreateOrUpdateModel model)
        {

                return Ok(await _iGroupPermissionRepository.Update(groupId, model));

        }
        [HttpPost("CreateNew")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreateNew(GroupPermissionCreateOrUpdateModel model)
        {
            try
            {
                return Ok(await _iGroupPermissionRepository.CreateNew(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("GroupDelete/{groupId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> RoleRemove(int groupId)
        {
            try
            {
                return Ok(await _iGroupPermissionRepository.Delete(groupId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("AddUser")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> AddUser([FromBody] GroupPermission_AddUserCreateModelcs model)
        {
            try
            {
                return Ok(await _iGroupPermissionRepository.AddUser(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetMembers/{groupId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetMembers(int groupId)
        {
            try
            {
                return Ok(await _iGroupPermissionRepository.GetAccountGroup(groupId));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("RemoveUser")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> RemoveUser([FromBody] GroupPermission_AddUserCreateModelcs model)
        {
            try
            {
                return Ok(await _iGroupPermissionRepository.RemoveUser(model));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
