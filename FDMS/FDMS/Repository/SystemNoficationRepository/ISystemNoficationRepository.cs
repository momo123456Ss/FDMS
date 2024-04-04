using FDMS.Model;

namespace FDMS.Repository.SystemNoficationRepository
{
    public interface ISystemNoficationRepository
    {
        Task CreateNew(string content);
        Task<APIResponse> GetAll(string searchString, string date);

    }
}
