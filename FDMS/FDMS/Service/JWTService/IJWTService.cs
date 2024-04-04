using FDMS.Entity;
using FDMS.Model;

namespace FDMS.Service.JWTService
{
    public interface IJWTService
    {
        Task<TokenModel> GenerateToken(Account account);
        Task<Account> ReadToken();
    }
}
