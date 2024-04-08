using FDMS.Model;

namespace FDMS.Repository.FlightDocumentRepository
{
    public interface IFlightDocumentRepository
    {
        Task<APIResponse> CreateNew(int flightId, FlightDocumentCreateModel model);
        Task<APIResponse> Delete(int documentId);


        Task<APIResponse> GetDocumentByFlightId(int flightId);
        Task<APIResponse> GetDocumentByGOStaff(string docType, string date, string searchString);

    }
}
