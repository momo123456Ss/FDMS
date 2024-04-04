using FDMS.Model;

namespace FDMS.Repository.GeneralRepository
{
    public interface IGeneralRepository
    {
        Task<APIResponse> EditGeneral(GeneralUpdateModel model);
        Task<APIResponse> GetGeneral();
    }
}
