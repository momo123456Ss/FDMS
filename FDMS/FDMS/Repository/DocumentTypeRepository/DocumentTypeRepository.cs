using AutoMapper;
using CloudinaryDotNet.Actions;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Repository.AccountSessionRepository;
using FDMS.Repository.SystemNoficationRepository;
using FDMS.Service.JWTService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace FDMS.Repository.DocumentTypeRepository
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly FDMSContext _context;
        private readonly IJWTService _jWTService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _iConfiguration;
        private readonly ISystemNoficationRepository _iSystemNoficationRepository;

        public DocumentTypeRepository(IJWTService jWTService, FDMSContext context,
            IMapper mapper, IConfiguration iConfiguration, ISystemNoficationRepository iSystemNoficationRepository)
        {
            _jWTService = jWTService;
            _context = context;
            _mapper = mapper;
            _iConfiguration = iConfiguration;
            _iSystemNoficationRepository = iSystemNoficationRepository;
        }

        public async Task<APIResponse> AddGroupPermission(DocumentType_AddGroupCreateOrUpdateModel model)
        {
            var user = await _jWTService.ReadToken();
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            List<string> groupName = new List<string>();
            foreach(var obj in model.GroupPermissionConfigurations)
            {
                bool groupExistsInDocument = await _context.DocumentType_Permissions
                        .AnyAsync(agp => agp.GroupPermissionId == obj.GroupPermissionId && agp.DocumentTypeId == model.DocumentTypeId);
                if (groupExistsInDocument)
                {
                    continue;
                }
                var newAdd = new DocumentType_Permission
                {
                    DocumentTypeId = (int)model.DocumentTypeId,
                    GroupPermissionId = (int)obj.GroupPermissionId,
                    ReadAndModify = (bool)obj.ReadAndModify,
                    ReadOnly = (bool)obj.ReadOnly,
                    NoPermission = (bool)obj.NoPermission
                };
                await _context.AddAsync(newAdd);
                var group = await _context.GroupPermissions.FirstOrDefaultAsync(g => g.GroupPermissionId.Equals(obj.GroupPermissionId));
                groupName.Add(group.GroupName);
            };
            await _context.SaveChangesAsync();
          
            var docType = await _context.DocumentTypes.FirstOrDefaultAsync(d => d.DocumentTypeId.Equals(model.DocumentTypeId));
            docType.TotalGroups = await _context.DocumentType_Permissions.Where(g => g.DocumentTypeId.Equals(model.DocumentTypeId)).CountAsync();            
            await _context.SaveChangesAsync();

            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
              $"đã thêm group permission:  {string.Join(", ", groupName)}, vào Document type:    {docType.Type} - {docType.DocumentTypeId}.");
            return new APIResponse
            {
                success = true,
                message = $"Add group permission to doc type.",
                msg = $"Add group permission to doc type."
            };
        }

        public async Task<APIResponse> ConfigGroupPermission(int docTypeId, int groupPId, DocumentType_ConfigGroupPermissionUpdateModel model)
        {
            var user = await _jWTService.ReadToken();
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            var dGroupP = await _context.DocumentType_Permissions
                .Include(g => g.GroupPermissionNavigation)
                .Include(dt => dt.DocumentTypeNavigation)
                .Where(d => d.DocumentTypeId.Equals(docTypeId) && d.GroupPermissionId.Equals(groupPId))
                .FirstOrDefaultAsync();
            dGroupP.ReadAndModify = model.ReadAndModify;
            dGroupP.ReadOnly = model.ReadOnly;
            dGroupP.NoPermission = model.NoPermission;
            await _context.SaveChangesAsync();


            var configType = "";
            if (model.ReadAndModify)
                configType = "ReadAndModify";
            else if (model.ReadOnly)
                configType = "ReadOnly";
            else if (model.NoPermission)
                configType = "NoPermission";

            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã config group permission: {dGroupP.GroupPermissionNavigation.GroupName} - {dGroupP.GroupPermissionNavigation.GroupPermissionId}, " +
                $"document type: {dGroupP.DocumentTypeNavigation.Type} - {dGroupP.DocumentTypeNavigation.DocumentTypeId}, " +
                $"loại config: {configType} - true.\"");
            return new APIResponse
            {
                success = true,
                message = "Config group permission.",
                msg = "Config group permission."
            };
        }

        public async Task<APIResponse> CreateNew(DocumentTypeCreateOrUpdateModel model)
        {
            var user = await _jWTService.ReadToken();
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            if (model == null || string.IsNullOrEmpty(model.Type) || string.IsNullOrEmpty(model.Note))
            {
                return new APIResponse
                {
                    success = false,
                    message = "Fill in all the information.",
                    msg = "Fill in all the information."
                };
            }
            var newDType = _mapper.Map<DocumentType>(model);
            newDType.CreateDate = DateTime.Now;
            newDType.TotalGroups = 0;
            newDType.Creator = user.Email;
            await _context.AddAsync(newDType);
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã tạo document type mới, document type: {newDType.Type} - note: {newDType.Note} - id: {newDType.DocumentTypeId}.");
            return new APIResponse
            {
                success = true,
                message = "Document type create.",
                msg = "Document type create."
            };
        }

        public async Task<APIResponse> GetAll()
        {
            var dType = await _context.DocumentTypes
                .Include(d => d.AccountNavigation).ThenInclude(a => a.RoleNavigation)
                .ToListAsync();
            return new APIResponse { 
                success= true ,
                message = "List document type.",
                msg = "List document type.",
                data = _mapper.Map<List<DocumentTypeViewModel>>(dType)
            };
        }

        public async Task<APIResponse> GetById(int documentTypeId)
        {
            var dType = await _context.DocumentTypes
                .Include(d => d.AccountNavigation).ThenInclude(a => a.RoleNavigation)
                .FirstOrDefaultAsync(d => d.DocumentTypeId.Equals(documentTypeId));
            return new APIResponse
            {
                success = true,
                message = "Document type.",
                msg = "Document type.",
                data = _mapper.Map<DocumentTypeViewModel>(dType)
            };
        }

        public async Task<APIResponse> GetGroupPermissionByDocTypeId(int DocumentTypeId)
        {
            var dGroupP = await _context.DocumentType_Permissions
                .Include(g => g.GroupPermissionNavigation)
                .Where(d => d.DocumentTypeId.Equals(DocumentTypeId))
                .ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List group permission.",
                msg = "List group permission.",
                data = _mapper.Map<List<DocumentType_GroupPermissionViewModel>>(dGroupP)
            };
        }

        public async Task<APIResponse> RemoveGroupPermission(DocumentType_RemoveGroupDeleteModel model)
        {
            var user = await _jWTService.ReadToken();
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            List<string> groupName = new List<string>();
            foreach (var groupId in model.GroupPermissionIds)
            {
                var group = await _context.DocumentType_Permissions
                    .FirstOrDefaultAsync(g => g.GroupPermissionId.Equals(groupId) && g.DocumentTypeId.Equals(model.DocumentTypeId));
                _context.DocumentType_Permissions.Remove(group);
                var groupRemove = await _context.GroupPermissions.FirstOrDefaultAsync(g => g.GroupPermissionId.Equals(group.GroupPermissionId));
                groupName.Add(groupRemove.GroupName);
            };
            await _context.SaveChangesAsync();

            var docType = await _context.DocumentTypes.FirstOrDefaultAsync(d => d.DocumentTypeId.Equals(model.DocumentTypeId));
            docType.TotalGroups = await _context.DocumentType_Permissions.Where(g => g.DocumentTypeId.Equals(model.DocumentTypeId)).CountAsync();
            await _context.SaveChangesAsync();

            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
              $"đã xóa group permission:  {string.Join(", ", groupName)}, khỏi Document type: {docType.Type} - {docType.DocumentTypeId}.");
            return new APIResponse
            {
                success = true,
                message = $"Remove group permission to doc type.",
                msg = $"Remove group permission to doc type."
            };
        }

        public async Task<APIResponse> Update(int documentTypeId, DocumentTypeCreateOrUpdateModel model)
        {
            var user = await _jWTService.ReadToken();
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            var dType = await _context.DocumentTypes.FirstOrDefaultAsync(d => d.DocumentTypeId.Equals(documentTypeId));
            foreach (var property in typeof(DocumentTypeCreateOrUpdateModel).GetProperties())
            {
                var modelValue = property.GetValue(model);
                if (modelValue != null)
                {
                    var documentTypeProperty = typeof(DocumentType).GetProperty(property.Name);
                    if (documentTypeProperty != null)
                    {
                        documentTypeProperty.SetValue(dType, modelValue);
                    }
                }
            }
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
               $"đã chỉnh sửa document type, document type {dType.Type} - {dType.DocumentTypeId}.");
            return new APIResponse
            {
                success = true,
                message = "Document type update.",
                msg = "Document type update."
            };
        }
    }
}
