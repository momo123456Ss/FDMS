using AutoMapper;
using CloudinaryDotNet.Actions;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Repository.AccountSessionRepository;
using FDMS.Repository.FDHistoryRepository;
using FDMS.Repository.SystemNoficationRepository;
using FDMS.Service.CloudinaryService;
using FDMS.Service.JWTService;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Linq;

namespace FDMS.Repository.FlightDocumentRepository
{
    public class FlightDocumentRepository : IFlightDocumentRepository
    {
        private readonly FDMSContext _context;
        private readonly IJWTService _jWTService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _iConfiguration;
        private readonly IFDRepository _iFDRepository;
        private readonly ICloudinaryService _cloudinaryService;
        public FlightDocumentRepository(IJWTService jWTService, FDMSContext context,
            IMapper mapper, IConfiguration iConfiguration,
            IFDRepository iFDRepository, ICloudinaryService cloudinaryService)
        {
            _jWTService = jWTService;
            _context = context;
            _mapper = mapper;
            _iConfiguration = iConfiguration;
            _iFDRepository = iFDRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<APIResponse> CreateNew(int flightId, FlightDocumentCreateModel model)
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
            var url = await _cloudinaryService.UploadCloudinary(model.newFile, _iConfiguration["Cloudinary_folder:flight-doc-folder"]);
            var newFD = _mapper.Map<FlightDocument>(model);
            newFD.VersionPatch = 0;
            newFD.CreatedDate = DateTime.UtcNow;
            newFD.FlightId = flightId;
            newFD.Creator = user.Email;
            newFD.FileType = Path.GetExtension(model.newFile.FileName);
            newFD.FileSize = $"{(double)model.newFile.Length / (1024 * 1024):0.##} MB";
            newFD.FileUrl = url;
            newFD.FileViewUrl = (Path.GetExtension(model.newFile.FileName) == ".mp3" || Path.GetExtension(model.newFile.FileName) == ".mp4" || Path.GetExtension(model.newFile.FileName) == ".pdf")
            ? url : String.Concat("https://docs.google.com/viewer?url=", url);
            await _context.AddAsync(newFD);
            await _context.SaveChangesAsync();

            foreach (var group in model.GroupPermissionIds)
            {
                var newFD_Group = new FlightDocument_GroupPermission
                {
                    GroupPermissionId = group,
                    FlightDocumentId = newFD.FlightDocumentId
                };
                await _context.AddAsync(newFD_Group);
            };
            await _context.SaveChangesAsync();
            var flight = await _context.Flights.FirstOrDefaultAsync(f => f.FlightId.Equals(flightId));
            flight.TotalDocuments = await _context.FlightDocuments.CountAsync(f => f.FlightId.Equals(flightId));
            await _context.SaveChangesAsync();

            await _iFDRepository.CreateNew($"{user.RoleNavigation.RoleName}: {user.Name}, địa chỉ hộp thư {user.Email} đã tạo mới " +
                $"tài liệu {newFD.FileName}.{newFD.FileType} cho chuyến bay {flight.FlightCode}{flight.FlightId.ToString("D3")}.");

            return new APIResponse
            {
                success = true,
                message = "Flight document create.",
                msg = "Flight document create."
            };
        }

        public async Task<APIResponse> Delete(int documentId)
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
            var doc = await _context.FlightDocuments.FirstOrDefaultAsync(d => d.FlightDocumentId.Equals(documentId));
            if (doc.VersionPatch.Equals(0) && 
                await _context.FlightDocuments
                .Where(d => d.FlightDocumentIdFK.Equals(doc.FlightDocumentId))
                .CountAsync() > 0)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Cant delete original document has been edited.",
                    msg = "Cant delete original document has been edited."
                };
            }
            var docGroup = await _context.FlightDocument_GroupPermissions
                .Where(d => d.FlightDocumentId.Equals(doc.FlightDocumentId))
                .ToListAsync();
            _context.FlightDocument_GroupPermissions.RemoveRange(docGroup);
            _context.FlightDocuments.Remove(doc); 
            await _context.SaveChangesAsync();

            var flight = await _context.Flights.FirstOrDefaultAsync(f => f.FlightId.Equals(doc.FlightId));
            flight.TotalDocuments = await _context.FlightDocuments.CountAsync(f => f.FlightId.Equals(doc.FlightId));
            await _context.SaveChangesAsync();
            await _iFDRepository.CreateNew($"{user.RoleNavigation.RoleName}: {user.Name}, địa chỉ hộp thư {user.Email} đã xóa " +
                $"tài liệu {doc.FileName}.{doc.FileType} cho chuyến bay {flight.FlightCode}{flight.FlightId.ToString("D3")}.");

            await _cloudinaryService.DeleteCloudinary(doc.FileUrl, _iConfiguration["Cloudinary_folder:flight-doc-folder"]);
            return new APIResponse
            {
                success = true,
                message = "Flight document delete.",
                msg = "Flight document delete."
            };
        }

        public async Task<APIResponse> GetDocumentByFlightId(int flightId)
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
            var docF = await _context.FlightDocuments
                .Include(dt => dt.DocumentTypeNavigation)
                .Include(f => f.FlightNavigation)
                .Include(c => c.AccountNavigation)
                .Where(f => f.FlightId.Equals(flightId))
                .ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List flight document.",
                msg = "List flight document.",
                data = _mapper.Map<List<FlightDocumentViewModel>>(docF)
            };
        }

        public async Task<APIResponse> GetDocumentByGOStaff(string docType, string date, string searchString)
        {
            var user = await _jWTService.ReadToken();
            var allD = _context.FlightDocuments
                .Include(dt => dt.DocumentTypeNavigation)
                .Include(c => c.AccountNavigation)
                .Include(f => f.FlightNavigation)
                .Where(c => c.Creator.Equals(user.Email))
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allD = allD.Where(a =>
                    a.FileName.ToLower().Contains(searchString) ||
                    (a.FlightNavigation.FlightCode.ToLower() + a.FlightNavigation.FlightId).Contains(searchString)
                );
            }
            if (!string.IsNullOrEmpty(date))
            {
                date = date.ToLower();
                allD = allD.Where(a =>
                    a.CreatedDate.ToString().ToLower().Contains(date)
                );
            }
            if (!string.IsNullOrEmpty(docType))
            {
                docType = docType.ToLower();
                allD = allD.Where(a =>
                   a.DocumentTypeNavigation.Type.Equals(docType) ||
                   a.DocumentTypeNavigation.DocumentTypeId.Equals(int.Parse(docType))
                );
            }
            allD = allD.OrderByDescending(sn => sn.UpdatedDate);
            var result = await allD.ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List flight document.",
                msg = "List flight document.",
                data = _mapper.Map<List<FlightDocumentViewModel>>(result)
            };
        }
    }
}
