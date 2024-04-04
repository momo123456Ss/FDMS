using FDMS.Model;

namespace FDMS.Repository.AccountRepository
{
    public interface IAccountRepository
    {
        Task<APIResponse> SignIn(SignInModel model);
        Task<APIResponse> GetByEmail(string email);
        Task<APIResponse> GetAll(string? searchString);
        Task<APIResponse> CreateAccount(AccountCreateModel model);
        Task<APIResponse> ChangeOwner(string email,string passwordConfirm);
        Task<APIResponse> GetOwner();
        Task<APIResponse> UpdateAccount(string email, AccountUpdateModel model);
        Task<APIResponse> RenewPassword(MailData model);
        Task<APIResponse> ChangePassword(AccountChangePasswordModel model);
    }
}
