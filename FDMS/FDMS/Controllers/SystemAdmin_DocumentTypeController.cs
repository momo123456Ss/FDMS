﻿using FDMS.Model;
using FDMS.Repository.AccountRepository;
using FDMS.Repository.DocumentTypeRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdmin_DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeRepository _iDocumentTypeRepository;

        public SystemAdmin_DocumentTypeController(IDocumentTypeRepository iDocumentTypeRepository)
        {
            this._iDocumentTypeRepository = iDocumentTypeRepository;
        }
        [HttpGet("GetDocumentType")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetDocumentType()
        {
            try
            {
                return Ok(await _iDocumentTypeRepository.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetDocumentTypeById/{id}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetDocumentTypeById(int id)
        {
            try
            {
                return Ok(await _iDocumentTypeRepository.GetById(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("CreateDocumentType")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> CreateDocumentType([FromBody] DocumentTypeCreateOrUpdateModel model)
        {
            try
            {
                return Ok(await _iDocumentTypeRepository.CreateNew(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateDocumentType/{id}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> UpdateDocumentType(int id, [FromBody] DocumentTypeCreateOrUpdateModel model)
        {
            try
            {
                return Ok(await _iDocumentTypeRepository.Update(id, model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("AddGroupPermission")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> AddGroupPermission([FromBody] DocumentType_AddGroupCreateOrUpdateModel model)
        {
            try
            {
                return Ok(await _iDocumentTypeRepository.AddGroupPermission(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("RemoveGroupPermission")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> RemoveGroupPermission([FromBody] DocumentType_RemoveGroupDeleteModel model)
        {
            try
            {
                return Ok(await _iDocumentTypeRepository.RemoveGroupPermission(model));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetGroupPermissionByDocumentTypeId/{id}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> GetGroupPermissionByDocumentTypeById(int id)
        {
            try
            {
                return Ok(await _iDocumentTypeRepository.GetGroupPermissionByDocTypeId(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("document-type-id/{docTypeId}/config-group-permission/{groupId}")]
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> ConfigGroupPermission(int docTypeId, int groupId, [FromBody] DocumentType_ConfigGroupPermissionUpdateModel model)
        {
            try
            {
                return Ok(await _iDocumentTypeRepository.ConfigGroupPermission(docTypeId, groupId, model));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
