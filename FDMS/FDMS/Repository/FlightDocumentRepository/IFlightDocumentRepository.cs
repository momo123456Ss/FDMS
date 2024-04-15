using FDMS.Model;

namespace FDMS.Repository.FlightDocumentRepository
{
    public interface IFlightDocumentRepository
    {
        Task<APIResponse> CreateNew(int flightId, FlightDocumentCreateModel model);
        Task<APIResponse> Delete(int documentId);
        Task<APIResponse> Update(int documentId, IFormFile file);

        Task<List<int>> GetListDocumentId(int flightId);
        Task<APIResponse> GetDocumentByFlightId(int flightId);
        Task<APIResponse> GetDocumentByGOStaff(string docType, string date, string searchString);
        Task<APIResponse> GetDocumentUpdatedVersion(int documentId);
        Task<APIResponse> GetDocumentById(int documentId);
        Task<APIResponse> GetOriginalDocumentByFlightId(int flightId);
        Task<APIResponse> GetUpdateDocumentByFlightId(int flightId);
        Task<APIResponse> GetLastVersionDocumentByFlightId(int flightId);
        Task<APIResponse> CountFlightDocumenyByFlightId_CMS(int flightId);
        Task<APIResponse> CountFlightDocumenyByFlightId_Mobile(int flightId);
        Task<APIResponse> GetUpdateFlightDocumentByAccount_Mobile(int flightId);

    }
}
