using FDMS.Model;

namespace FDMS.Repository.FDHistoryRepository
{
    public interface IFDRepository
    {
        Task CreateNew(string content);
        Task<APIResponse> GetAll(string searchString, string date);
    }
}
