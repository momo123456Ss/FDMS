using FDMS.Model;

namespace FDMS.Repository.FlightRepository
{
    public interface IFlightRepository
    {
        Task<APIResponse> CreateFlight(FlightCreateModel model);
        Task<APIResponse> GetAll(string? searchString, string? date);
        //Add account
        Task<APIResponse> AddAccount(FlightAddAccount model);
        Task<APIResponse> RemoveAccount(FlightAddAccount model);
        Task<APIResponse> GetFlightAccount();
        Task<APIResponse> FlightConfirm(int flightId, FlightConfirmModel model);
        Task<APIResponse> GetById(int flightId);
        Task<APIResponse> GetCurrentFlight();


    }
}
