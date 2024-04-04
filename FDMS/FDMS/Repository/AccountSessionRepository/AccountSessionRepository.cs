using AutoMapper;
using CloudinaryDotNet;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Repository.SystemNoficationRepository;
using FDMS.Service.JWTService;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FDMS.Repository.AccountSessionRepository
{
    public class AccountSessionRepository : IAccountSessionRepository
    {
        private readonly FDMSContext _context;
        private readonly IJWTService _jWTService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _iConfiguration;
        private readonly ISystemNoficationRepository _iSystemNoficationRepository;

        public AccountSessionRepository(IJWTService jWTService, FDMSContext context, IMapper mapper, 
            IConfiguration iConfiguration, ISystemNoficationRepository iSystemNoficationRepository)
        {
            _jWTService = jWTService;
            _context = context;
            _mapper = mapper;
            _iConfiguration = iConfiguration;
            _iSystemNoficationRepository = iSystemNoficationRepository;
        }

        public async Task Create(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(tokenString) as JwtSecurityToken;
            var emailClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            var accountEmail = emailClaim.Value;

            var newAccountSession = new AccountSession
            {
                AccountEmail = accountEmail,
                AccessToken = tokenString,
                NotBefore = token.ValidFrom,
                ExpirationTime = token.ValidTo,
                IssuedAt = token.IssuedAt
            };
            await _context.AddAsync(newAccountSession);
            await _context.SaveChangesAsync();
        }

        public async Task<APIResponse> GetAll()
        {
            var allAccountSession = _context.Accounts
                .Include(r => r.RoleNavigation)
                .AsQueryable();
            ICollection<AccountSession> AccountSessions = await _context.AccountSessions
                    .OrderByDescending(acs => acs.ExpirationTime)
                    .Where(acs => acs.ExpirationTime > DateTime.UtcNow)
                    .ToListAsync();
            ICollection<string> emailList = AccountSessions.Select(acs => acs.AccountEmail).Distinct().ToList();

            allAccountSession = allAccountSession.Where(a => emailList.Contains(a.Email));
            var result = await allAccountSession.ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "Account in session.",
                msg = "Account in session.",
                data = _mapper.Map<List<AccountViewModel>>(result)
            };
        }

        public async Task<APIResponse> Lock(List<string> emails)
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
            foreach (var email in emails)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(email));
                account.IsActived = false;
                await _context.SaveChangesAsync();
                await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã khóa tài khoản tên {account.Name} - {account.Email}.");
            };
            return new APIResponse
            {
                success = true,
                message = "Account lock.",
                msg = "Account lock."
            };
        }

        public async Task<APIResponse> Unlock(List<string> emails)
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
            foreach (var email in emails) {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(email));
                account.IsActived = true;
                await _context.SaveChangesAsync();
                await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
                $"đã mở khóa tài khoản tên {account.Name} - {account.Email}.");
            };
            return new APIResponse
            {
                success = true,
                message = "Account unlock.",
                msg = "Account unlock."
            };
        }
    }
}
