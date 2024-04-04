using AutoMapper;
using CloudinaryDotNet.Actions;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Repository.SystemNoficationRepository;
using FDMS.Service.JWTService;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FDMS.Repository.GroupPermission
{
    public class GroupPermissionRepository : IGroupPermissionRepository
    {
        private readonly FDMSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTService _jWTService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _iConfiguration;
        private readonly ISystemNoficationRepository _iSystemNoficationRepository;

        public GroupPermissionRepository(IHttpContextAccessor httpContextAccessor, IJWTService jWTService
            , FDMSContext context, IMapper mapper, IConfiguration iConfiguration, ISystemNoficationRepository iSystemNoficationRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _jWTService = jWTService;
            _context = context;
            _mapper = mapper;
            _iConfiguration = iConfiguration;
            _iSystemNoficationRepository = iSystemNoficationRepository;
        }
        private async Task<int> GenerateGroupId()
        {
            int count = await _context.GroupPermissions.CountAsync();
            return count + 1;
        }
        public async Task<APIResponse> CreateNew(GroupPermissionCreateOrUpdateModel model)
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
            if (model == null || string.IsNullOrEmpty(model.GroupName))
            {
                return new APIResponse
                {
                    success = false,
                    message = "Fill in all the information.",
                    msg = "Fill in all the information."
                };
            }
            if (model.Note == null)
            {
                model.Note = "--";
            }
            var newGroup = _mapper.Map<FDMS.Entity.GroupPermission>(model);
            newGroup.TotalMembers = 0;
            newGroup.CreatedDate = DateTime.Now;
            newGroup.Creator = user.Email;
            await _context.AddAsync(newGroup);
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã tạo group permission mới, tên nhóm {newGroup.GroupName} - {newGroup.GroupPermissionId}.");

            return new APIResponse
            {
                success = true,
                message = "Group successfully created.",
                msg = "Group successfully created."
            };
        }

        public async Task<APIResponse> Delete(int groupId)
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
            var group = await _context.GroupPermissions.FirstOrDefaultAsync(g => g.GroupPermissionId == groupId);
            _context.GroupPermissions.Remove(group);
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã xóa group permission, tên nhóm {group.GroupName} - {group.GroupPermissionId}.");
            return new APIResponse
            {
                success = true,
                message = "Group remove.",
                msg = "Group remove."
            };
        }

        public async Task<APIResponse> GetById(int groupId)
        {
            var group = await _context.GroupPermissions
                .Include(a => a.AccountNavigation)
                .ThenInclude(r => r.RoleNavigation)
                .FirstOrDefaultAsync(g => g.GroupPermissionId == groupId);
            return new APIResponse
            {
                success = true,
                message = "Group found.",
                msg = "Group found.",
                data = _mapper.Map<GroupPermissionViewModel>(group)
            };
        }

        public async Task<APIResponse> GetAll()
        {
            var allGroup = await _context.GroupPermissions
                .Include(g => g.AccountNavigation)
                .ThenInclude(r => r.RoleNavigation)
                .ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "Group found.",
                msg = "Group found.",
                data = _mapper.Map<List<GroupPermissionViewModel>>(allGroup)
            };
        }

        public async Task<APIResponse> Update(int groupId, GroupPermissionCreateOrUpdateModel model)
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
            var group = await _context.GroupPermissions.FirstOrDefaultAsync(g => g.GroupPermissionId.Equals(groupId));
            foreach (var property in typeof(GroupPermissionCreateOrUpdateModel).GetProperties())
            {
                var modelValue = property.GetValue(model);
                if (modelValue != null)
                {
                    var groupProperty = typeof(FDMS.Entity.GroupPermission).GetProperty(property.Name);
                    if (groupProperty != null)
                    {
                        groupProperty.SetValue(group, modelValue);
                    }
                }
            }
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã chỉnh sửa group permission, tên nhóm {group.GroupName} - {group.GroupPermissionId}.");
            return new APIResponse
            {
                success = true,
                message = "Update group.",
                msg = "Update group."
            };
        }

        public async Task<APIResponse> AddUser(GroupPermission_AddUserCreateModelcs model)
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
            foreach (var email in model.emails)
            {
                bool emailExistsInGroup = await _context.Account_GroupPermissions
                        .AnyAsync(agp => agp.GroupPermissionId == model.GroupPermissionId && agp.AccountEmail == email);
                if (emailExistsInGroup)
                {
                    continue;
                }
                var addAccount = new Account_GroupPermission
                {
                    GroupPermissionId = model.GroupPermissionId,
                    AccountEmail = email
                };
                await _context.AddAsync(addAccount);
            };
            await _context.SaveChangesAsync();
            var emailExists = model.emails.Where(email =>
                    _context.Accounts.Any(a => a.Email == email)).ToList();

            var group = await _context.GroupPermissions.FirstOrDefaultAsync(g => g.GroupPermissionId.Equals(model.GroupPermissionId));
            group.TotalMembers = await _context.Account_GroupPermissions.Where(g => g.GroupPermissionId.Equals(model.GroupPermissionId)).CountAsync();
            await _context.SaveChangesAsync();

            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
               $"đã thêm account:  {string.Join(", ", emailExists)}, vào nhóm {group.GroupName} - {group.GroupPermissionId}.");
            return new APIResponse
            {
                success = true,
                message = $"Add user to group.",
                msg = $"Add user to group."
            };
        }

        public async Task<APIResponse> GetAccountGroup(int groupId)
        {
            ICollection<Account_GroupPermission> AccountGroups = await _context.Account_GroupPermissions
                    .OrderByDescending(a => a.Id)
                    .Where(ag => ag.GroupPermissionId.Equals(groupId))
                    .ToListAsync();
            ICollection<string> emailList = AccountGroups.Select(ag => ag.AccountEmail).Distinct().ToList();
            var members = await _context.Accounts.Include(a => a.RoleNavigation)
                .Where(a => emailList.Contains(a.Email))
                .ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "Members.",
                msg = "Members.",
                data = _mapper.Map<List<AccountViewModel>>(members)
            };
        }

        public async Task<APIResponse> RemoveUser(GroupPermission_AddUserCreateModelcs model)
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
            foreach (var email in model.emails)
            {
                var obj = await _context.Account_GroupPermissions
                    .FirstOrDefaultAsync(g => g.GroupPermissionId.Equals(model.GroupPermissionId) && g.AccountEmail.Equals(email));
                _context.Account_GroupPermissions.Remove(obj);
            };
            await _context.SaveChangesAsync();

            var emailExists = model.emails.Where(email =>
                    _context.Accounts.Any(a => a.Email == email)).ToList();

            var group = await _context.GroupPermissions.FirstOrDefaultAsync(g => g.GroupPermissionId.Equals(model.GroupPermissionId));
            group.TotalMembers = await _context.Account_GroupPermissions.Where(g => g.GroupPermissionId.Equals(model.GroupPermissionId)).CountAsync();
            await _context.SaveChangesAsync();

            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
               $"đã gỡ bỏ account:  {string.Join(", ", emailExists)}, khỏi nhóm {group.GroupName} - {group.GroupPermissionId}.");
            return new APIResponse
            {
                success = true,
                message = $"Remove user in group.",
                msg = $"Remove user in group."
            };
        }
    }
}
