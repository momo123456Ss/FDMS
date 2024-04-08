using AutoMapper;
using CloudinaryDotNet.Actions;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Repository.SystemNoficationRepository;
using FDMS.Service.JWTService;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace FDMS.Repository.FlightRepository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FDMSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTService _jWTService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _iConfiguration;
        private readonly ISystemNoficationRepository _iSystemNoficationRepository;

        public FlightRepository(IHttpContextAccessor httpContextAccessor, IJWTService jWTService
            , FDMSContext context, IMapper mapper, IConfiguration iConfiguration, ISystemNoficationRepository iSystemNoficationRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _jWTService = jWTService;
            _context = context;
            _mapper = mapper;
            _iConfiguration = iConfiguration;
            _iSystemNoficationRepository = iSystemNoficationRepository;
        }
        private string GetFirstLetters(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b\w");
            string result = "";
            foreach (Match match in matches)
            {
                result += match.Value;
            }

            return result;
        }

        public async Task<APIResponse> CreateFlight(FlightCreateModel model)
        {
            var user = await _jWTService.ReadToken();
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            var newFlight = _mapper.Map<Flight>(model);
            newFlight.FlightCode = _iConfiguration["FlightCode:vietjet-air"];
            newFlight.Route = GetFirstLetters(model.PointOfLoading).ToUpper() + " - " + GetFirstLetters(model.PointOfUnLoading).ToUpper();
            newFlight.TotalDocuments = 0;
            await _context.AddAsync(newFlight);
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
               $"đã thêm chuyến bay, {newFlight.FlightCode}{newFlight.FlightId.ToString("D3")}.");
            return new APIResponse
            {
                success = true,
                message = "Flight created",
                msg = "Flight created"
            };
        }

        public async Task<APIResponse> GetAll(string? searchString, string? date)
        {
            var allFlight = _context.Flights.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allFlight = allFlight.Where(f =>
                    (f.FlightCode.ToLower() + f.FlightId.ToString()).Contains(searchString) ||
                    f.Route.ToLower().Contains(searchString) ||
                    f.PointOfUnLoading.ToLower().Contains(searchString) ||
                    f.PointOfLoading.ToLower().Contains(searchString)
                );
            }
            if (!string.IsNullOrEmpty(date))
            {
                DateTime.TryParse(date, out DateTime searchDate);
                allFlight = allFlight.Where(f => f.DepartureDate.Date == searchDate.Date);
            }
            allFlight = allFlight.OrderByDescending(f => f.FlightId);
            var result = await allFlight.ToListAsync();
            return new APIResponse { 
                success = true, 
                message = "Flight found.",
                msg = "Flight found.",
                data = _mapper.Map<List<FilghtViewModel>>(result)
            };
        }

        public async Task<APIResponse> AddAccount(FlightAddAccount model)
        {
            var user = await _jWTService.ReadToken();
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            foreach (var email in model.emails)
            {
                bool emailExistsInGroup = await _context.Flight_Accounts
                        .AnyAsync(agp => agp.FlightId == model.FlightId && agp.AccountEmail == email);
                if (emailExistsInGroup)
                {
                    continue;
                }
                var addAccount = new Flight_Account
                {
                    FlightId = model.FlightId,
                    AccountEmail = email
                };
                await _context.AddAsync(addAccount);
            };
            var emailExists = model.emails.Where(email =>
                    _context.Accounts.Any(a => a.Email == email)).ToList();
            var flight = await _context.Flights.FirstOrDefaultAsync(g => g.FlightId.Equals(model.FlightId));

            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
              $"đã phân công chuyến bay account:  {string.Join(", ", emailExists)}, Flight no {flight.FlightCode}{flight.FlightId.ToString("D3")}.");
            return new APIResponse
            {
                success = true,
                message = "Flight add account.",
                msg = "Flight add account."
            };
        }

        public async Task<APIResponse> RemoveAccount(FlightAddAccount model)
        {
             var user = await _jWTService.ReadToken();
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            foreach (var email in model.emails)
            {
                var obj = await _context.Flight_Accounts
                    .FirstOrDefaultAsync(g => g.FlightId.Equals(model.FlightId) && g.AccountEmail.Equals(email));
                _context.Flight_Accounts.Remove(obj);
            };
            await _context.SaveChangesAsync();
            var emailExists = model.emails.Where(email =>
                   _context.Accounts.Any(a => a.Email == email)).ToList();
            var flight = await _context.Flights.FirstOrDefaultAsync(g => g.FlightId.Equals(model.FlightId));
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
              $"đã bỏ phân công chuyến bay account:  {string.Join(", ", emailExists)}, Flight no {flight.FlightCode}{flight.FlightId.ToString("D3")}.");
            return new APIResponse
            {
                success = true,
                message = "Flight remove account.",
                msg = "Flight remove account."
            };
        }

        public async Task<APIResponse> GetFlightAccount()
        {
            var user = await _jWTService.ReadToken();

            var allFlight = _context.Flights.AsQueryable();
            ICollection<Flight_Account> flight_Accounts = await _context.Flight_Accounts
                    .OrderByDescending(acs => acs.Flight_AccountId)
                    .Where(f => f.AccountEmail.Equals(user.Email))
                    .ToListAsync();
            ICollection<int> flightList = flight_Accounts.Select(acs => acs.FlightId).Distinct().ToList();
            allFlight = allFlight.Where(a => flightList.Contains(a.FlightId));
            allFlight = allFlight.Where(f => f.DepartureDate.Date == DateTime.UtcNow.Date);
            var result = await allFlight.ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "Flight found.",
                msg = "Flight found.",
                data = _mapper.Map<List<FilghtViewModel>>(result)
            };
        }

        public async Task<APIResponse> FlightConfirm(int flightId, FlightConfirmModel model)
        {
            var user = await _jWTService.ReadToken();
            if (!user.RoleNavigation.RoleId.Equals(_iConfiguration["Role:owner-id"]) || !user.RoleNavigation.RoleId.Equals(_iConfiguration["Role:admin-id"]))
            {
                var fAccount = await _context.Flight_Accounts
                .Where(f => f.FlightId.Equals(flightId))
                .Where(f => f.AccountEmail.Equals(user.Email))
                .FirstOrDefaultAsync();
                if (fAccount == null)
                {
                    return new APIResponse
                    {
                        success = false,
                        message = "The account is not in charge.",
                        msg = "The account is not in charge."
                    };
                }
            }
            var flight = await _context.Flights.FirstOrDefaultAsync(f => f.FlightId.Equals(flightId));
            flight.Signature = model.Signature;
            flight.AccountConfirm = user.Email;
            flight.IsConfirm = model.IsConfirm;
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} " +
               $"đã confirm chuyến bay, {flight.FlightCode}{flight.FlightId.ToString("D3")}.");
            return new APIResponse
            {
                success = false,
                message = "Flight is confirm.",
                msg = "Flight is confirm."
            };
        }

        public async Task<APIResponse> GetById(int flightId)
        {
            var f = await _context.Flights.FirstOrDefaultAsync(f => f.FlightId.Equals(flightId));
            return new APIResponse
            {
                success = true,
                message = "Flight exists.",
                msg = "Flight exists.",
                data = _mapper.Map<FilghtViewModel>(f)
            };
        }
    }
}
