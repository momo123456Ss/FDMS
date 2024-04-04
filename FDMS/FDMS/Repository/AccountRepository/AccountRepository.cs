using AutoMapper;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Repository.AccountSessionRepository;
using FDMS.Repository.SystemNoficationRepository;
using FDMS.Service.JWTService;
using FDMS.Service.MailService;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace FDMS.Repository.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FDMSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTService _jWTService;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IConfiguration _iConfiguration;
        private readonly ISystemNoficationRepository _iSystemNoficationRepository;
        private readonly IAccountSessionRepository _iAccountSessionRepository;
        public AccountRepository(IHttpContextAccessor httpContextAccessor, IJWTService jWTService, FDMSContext context, 
            IMapper mapper, IMailService mailService, IConfiguration iConfiguration, 
            ISystemNoficationRepository iSystemNoficationRepository, IAccountSessionRepository iAccountSessionRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _jWTService = jWTService;
            _context = context;
            _mapper = mapper;
            _mailService = mailService;
            _iConfiguration = iConfiguration;
            _iSystemNoficationRepository = iSystemNoficationRepository;
            _iAccountSessionRepository = iAccountSessionRepository;
        }
        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+";
            var randomChars = new char[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[length];
                rng.GetBytes(bytes);
                for (int i = 0; i < length; i++)
                {
                    randomChars[i] = validChars[bytes[i] % validChars.Length];
                }
            }

            return new string(randomChars);
        }
        public async Task<APIResponse> CreateAccount(AccountCreateModel model)
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
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == model.RoleId);
            if (role == null)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Role not found",
                    msg = "Role not found"
                };
            }
            if (role.RoleId.Equals(_iConfiguration["Role:owner-id"]))
            {
                var owner = await _context.Accounts.Include(r => r.RoleNavigation).FirstOrDefaultAsync(a => a.RoleNavigation.RoleId.Equals(_iConfiguration["Role:owner-id"]));
                if(owner != null)
                {
                    return new APIResponse
                    {
                        success = false,
                        message = "Only one owner",
                        msg = "Only one owner"
                    };
                }
            }
            if (await _context.Accounts.AnyAsync(u => u.Email == model.Email))
            {
                return new APIResponse
                {
                    success = false,
                    message = "Email exists",
                    msg = "Email exists"
                };
            }         
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var newAccount = _mapper.Map<Account>(model);
            newAccount.Password = hashedPassword;
            newAccount.Avatar = "https://res.cloudinary.com/dicxcmntw/image/upload/v1712037171/FDMS/fdms_avatar_folder/dscccnnawnzce1uzyzig.png";
            await _context.AddAsync(newAccount);
            await _context.SaveChangesAsync();


            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {newAccount.Name}, địa chỉ hộp thư {newAccount.Email} đã được tạo.");
            return new APIResponse
            {
                success = true,
                message = "Account successfully created.",
                msg = "Account successfully created."
            };
        }

        public async Task<APIResponse> RenewPassword(MailData model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email.Equals(model.ReceiverEmail));
            if (user == null)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Email not found.",
                    msg = "Email not found."
                };
            }
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            var password = GenerateRandomPassword(12); // Tạo mật khẩu ngẫu nhiên có độ dài 12 ký tự

            // Mã hóa mật khẩu mới
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            // Cập nhật mật khẩu mới cho người dùng
            user.Password = hashedPassword;
            _context.Accounts.Update(user);
            await _context.SaveChangesAsync();
            model.ReceiverName = user.Name;
            model.Body = password;
            await _mailService.SendMail(model);
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} đã gửi yêu cầu tạo mới mật khẩu.");
            return new APIResponse
            {
                success = true,
                message = "Sent renew password.",
                msg = "Sent renew password."
            };
        }

        public async Task<APIResponse> SignIn(SignInModel model)
        {
            var user = _context.Accounts.SingleOrDefault(u => u.Email.Equals(model.Email));
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return new APIResponse
                {
                    success = false,
                    message = "Invalid username/password",
                    msg = "Invalid username/password"
                };
            }
            else if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            else
            {
                var token = await _jWTService.GenerateToken(user);
                await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} đã đăng nhập vào hệ thống.");
                await _iAccountSessionRepository.Create(token.AccessToken);
                return new APIResponse
                {
                    success = true,
                    message = "Authenticate success",
                    msg = "Authenticate success",
                    data = token
                };
            }
        }

        public async Task<APIResponse> UpdateAccount(string email, AccountUpdateModel model)
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
            var account = await _context.Accounts.FirstOrDefaultAsync(u => u.Email.Equals(email));
            if (account == null)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Email not exists",
                    msg = "Email not exists"
                };
            }
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId.Equals(model.RoleId));
            if (role == null)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Role not exists",
                    msg = "Role not exists"
                };
            }
            foreach (var property in typeof(AccountUpdateModel).GetProperties())
            {
                var modelValue = property.GetValue(model);
                if (modelValue != null)
                {
                    var accountProperty = typeof(Account).GetProperty(property.Name);
                    if (accountProperty != null)
                    {
                        accountProperty.SetValue(account, modelValue);
                    }
                }
            }
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {account.Name}, địa chỉ hộp thư {account.Email} đã được chỉnh sửa thông tin.");
            return new APIResponse
            {
                success = true,
                message = "Update account success.",
                msg = "Update account success."
            };
        }

        public async Task<APIResponse> ChangePassword(AccountChangePasswordModel model)
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
            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.Password))
            {
                return new APIResponse
                {
                    success = false,
                    message = "Invalid current password.",
                    msg = "Invalid current password."
                };
            }
            if (model.NewPassword.Equals(model.CurrentPassword))
            {
                return new APIResponse
                {
                    success = false,
                    message = "New password must be different from the current password.",
                    msg = "New password must be different from the current password."
                };
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.Password = hashedPassword;
            _context.Accounts.Update(user);
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} đã đổi mật khẩu.");
            return new APIResponse
            {
                success = true,
                message = "Password change success.",
                msg = "Password change success."
            };
        }

        public async Task<APIResponse> GetByEmail(string email)
        {           
            var account = await _context.Accounts.Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
            return new APIResponse
            {
                success = true,
                message = "Account found.",
                msg = "Account found.",
                data = _mapper.Map<AccountViewModel>(account)
            };
        }

        public async Task<APIResponse> GetAll(string? searchString)
        {
            var allAccount = _context.Accounts
                .Include(u => u.RoleNavigation)
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allAccount = allAccount.Where(a =>
                    a.Name.ToLower().Contains(searchString) ||
                    a.Email.ToLower().Contains(searchString) ||
                    a.RoleNavigation.RoleName.ToLower().Contains(searchString)
                );
            }
            allAccount = allAccount.OrderBy(r => r.RoleNavigation.RoleName);
            var result = await allAccount.ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List account found.",
                msg = "List account found.",
                data = _mapper.Map<List<AccountViewModel>>(result)
            };
        }

        public async Task<APIResponse> ChangeOwner(string email, string passwordConfirm)
        {
            var onwer = await _jWTService.ReadToken();
            if (!BCrypt.Net.BCrypt.Verify(passwordConfirm, onwer.Password))
            {
                return new APIResponse
                {
                    success = false,
                    message = "Invalid password.",
                    msg = "Invalid password."
                };
            }
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(email));
            if (account == null)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Email not exists",
                    msg = "Email not exists"
                };
            }
            account.RoleId = _iConfiguration["Role:owner-id"];
            onwer.RoleId = _iConfiguration["Role:non-id"];
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {onwer.Name}, địa chỉ hộp thư {onwer.Email} đã đổi owner, owner mới hiện là {account.Name}, địa chỉ hộp thư {account.Email}.");
            return new APIResponse
            {
                success = false,
                message = "Updated owner.",
                msg = "Updated owner."
            };
        }

        public async Task<APIResponse> GetOwner()
        {
            var owner = await _context.Accounts.Include(r => r.RoleNavigation).FirstOrDefaultAsync(a => a.RoleNavigation.RoleId.Equals(_iConfiguration["Role:owner-id"]));
            return new APIResponse
            {
                success = false,
                message = "Owner found.",
                msg = "Owner found.",
                data = _mapper.Map<AccountViewModel>(owner)
            };
        }
    }
}
