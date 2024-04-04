using FDMS.Model;

namespace FDMS.Repository.RoleRepository
{
    public interface IRoleRepository
    {
        Task<APIResponse> GetAll();
        Task<APIResponse> CreateNew(RoleCreateOrUpdateModel model);
        Task<APIResponse> Update(string roleId, RoleCreateOrUpdateModel model);
        Task<APIResponse> Delete(string roleId);
    }
}
