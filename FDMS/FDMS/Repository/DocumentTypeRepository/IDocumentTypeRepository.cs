using FDMS.Model;

namespace FDMS.Repository.DocumentTypeRepository
{
    public interface IDocumentTypeRepository
    {
        Task<APIResponse> CreateNew(DocumentTypeCreateOrUpdateModel model);
        Task<APIResponse> Update(int documentTypeId, DocumentTypeCreateOrUpdateModel model);
        Task<APIResponse> GetAll();
        Task<APIResponse> GetById(int documentTypeId);

        Task<APIResponse> AddGroupPermission(DocumentType_AddGroupCreateOrUpdateModel model);
        Task<APIResponse> RemoveGroupPermission(DocumentType_RemoveGroupDeleteModel model);
        Task<APIResponse> ConfigGroupPermission(int docTypeId, int groupPId, DocumentType_ConfigGroupPermissionUpdateModel model);
        Task<APIResponse> GetGroupPermissionByDocTypeId(int DocumentTypeId);

    }
}
