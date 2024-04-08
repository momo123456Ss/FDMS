using FDMS.Model;

namespace FDMS.Repository.GroupPermission
{
    public interface IGroupPermissionRepository
    {
        Task<APIResponse> CreateNew(GroupPermissionCreateOrUpdateModel model);
        Task<APIResponse> Update(int groupId, GroupPermissionCreateOrUpdateModel model);
        Task<APIResponse> Delete(int groupId);
        Task<APIResponse> GetById(int groupId);
        Task<APIResponse> GetAll();

        //Add user
        Task<APIResponse> AddUser(GroupPermission_AddUserCreateModelcs model);
        Task<APIResponse> GetAccountGroup(int groupId);
        Task<APIResponse> RemoveUser(GroupPermission_AddUserCreateModelcs model);


    }
}
