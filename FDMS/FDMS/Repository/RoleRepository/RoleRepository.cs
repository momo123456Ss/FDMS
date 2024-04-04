using AutoMapper;
using CloudinaryDotNet;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Repository.SystemNoficationRepository;
using FDMS.Service.JWTService;
using FDMS.Service.MailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace FDMS.Repository.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly FDMSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTService _jWTService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _iConfiguration;
        private readonly ISystemNoficationRepository _iSystemNoficationRepository;

        public RoleRepository(IHttpContextAccessor httpContextAccessor, IJWTService jWTService, FDMSContext context
            , IMapper mapper, IConfiguration iConfiguration, ISystemNoficationRepository iSystemNoficationRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _jWTService = jWTService;
            _context = context;
            _mapper = mapper;
            _iConfiguration = iConfiguration;
            _iSystemNoficationRepository = iSystemNoficationRepository;
        }
        private async Task<string> GenerateRoleId()
        {
            int roleCount = await _context.Roles.CountAsync();
            return $"VJR{roleCount + 1:000}";
        }
        public async Task<APIResponse> CreateNew(RoleCreateOrUpdateModel model)
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
            if (model == null || string.IsNullOrEmpty(model.RoleName) || string.IsNullOrEmpty(model.Description))
            {
                return new APIResponse
                {
                    success = false,
                    message = "Fill in all the information.",
                    msg = "Fill in all the information."
                };
            }
            var newRole = _mapper.Map<Role>(model);
            newRole.RoleId = await GenerateRoleId(); 
            await _context.AddAsync(newRole);
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã tạo mới role, tên role {newRole.RoleName} - {newRole.RoleId}.");
            return new APIResponse
            {
                success = true,
                message = "Role successfully created.",
                msg = "Role successfully created."
            };
        }
        public async Task<APIResponse> GetAll()
        {
            var allRole = await _context.Roles.ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List role found.",
                msg = "List role found.",
                data = _mapper.Map<List<RoleViewModel>>(allRole)
            };
        }

        public async Task<APIResponse> Update(string roleId, RoleCreateOrUpdateModel model)
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
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId.Equals(roleId));
            foreach (var property in typeof(RoleCreateOrUpdateModel).GetProperties())
            {
                var modelValue = property.GetValue(model);
                if (modelValue != null)
                {
                    var roleProperty = typeof(Role).GetProperty(property.Name);
                    if (roleProperty != null)
                    {
                        roleProperty.SetValue(role, modelValue);
                    }
                }
            }
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã chỉnh sửa role, tên role {role.RoleName} - {role.RoleId}.");
            return new APIResponse
            {
                success = true,
                message = "Update role.",
                msg = "Update role."
            };
        }

        public async Task<APIResponse> Delete(string roleId)
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
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId.Equals(roleId));
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã xóa role, tên role {role.RoleName} - {role.RoleId}.");
            return new APIResponse { 
                success = false ,
                message = "Role remove.",
                msg = "Role remove."
            };
        }
    }
}
