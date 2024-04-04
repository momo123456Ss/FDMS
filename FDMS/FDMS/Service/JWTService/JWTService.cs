using AutoMapper;
using FDMS.Entity;
using FDMS.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FDMS.Service.JWTService
{
    public class JWTService : IJWTService
    {
        private FDMSContext _context;
        private readonly IConfiguration _iConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JWTService(IConfiguration iConfiguration, FDMSContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._iConfiguration = iConfiguration;
            this._context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenModel> GenerateToken(Entity.Account account)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_iConfiguration["JWT:Secret"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Email", account.Email),
                    new Claim(ClaimTypes.Name, account.Name),
                    new Claim(JwtRegisteredClaimNames.Email, account.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //roles
                    new Claim("RoleId", account.RoleId.ToString()),
                    new Claim(ClaimTypes.Role, await _context.Roles
                                .Where(r => r.RoleId == account.RoleId)
                                .Select(r => r.RoleName.ToLower())
                                .FirstOrDefaultAsync()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _iConfiguration["JWT:ValidIssuer"], // Issuer của token
                Audience = _iConfiguration["JWT:ValidAudience"], // Audience của token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256),

            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var accessToken = jwtTokenHandler.WriteToken(token);
            return new TokenModel
            {
                AccessToken = accessToken
            };
        }

        public async Task<Account> ReadToken()
        {
            var user = await _context.Accounts.Include(role => role.RoleNavigation)
                                       .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            return user;
        }
    }
}
