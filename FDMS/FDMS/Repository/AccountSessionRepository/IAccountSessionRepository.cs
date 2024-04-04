using FDMS.Model;

namespace FDMS.Repository.AccountSessionRepository
{
    public interface IAccountSessionRepository
    {
        Task Create(string tokentokenString);
        Task<APIResponse> GetAll();
        Task<APIResponse> Unlock(List<string> emails);
        Task<APIResponse> Lock(List<string> emails);

    }
}
