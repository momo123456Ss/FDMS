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
using MimeKit;
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

        private bool IsEmailInReadOnlyGroups(string email)
        {
            var readOnlyGroups = _context.Account_GroupPermissions
                .Where(agp => agp.AccountEmail == email)
                .Join(_context.DocumentType_Permissions,
                    agp => agp.GroupPermissionId,
                    dtp => dtp.GroupPermissionId,
                    (agp, dtp) => new { dtp.DocumentTypeId, dtp.ReadOnly, dtp.ReadAndModify })
                .Where(x => x.ReadOnly || x.ReadAndModify)
                .Select(x => x.DocumentTypeId)
                .ToList();
            return readOnlyGroups.Any();
        }
        private bool IsEmailInReadAndModifyGroups(string email)
        {
            var readOnlyGroups = _context.Account_GroupPermissions
                .Where(agp => agp.AccountEmail == email)
                .Join(_context.DocumentType_Permissions,
                    agp => agp.GroupPermissionId,
                    dtp => dtp.GroupPermissionId,
                    (agp, dtp) => new { dtp.DocumentTypeId, dtp.ReadAndModify })
                .Where(x => x.ReadAndModify)
                .Select(x => x.DocumentTypeId)
                .ToList();
            return readOnlyGroups.Any();
        }
        private bool IsEmailInFlightDocumentGroups(string email, int flightDocumentId)
        {
            var documentGroups = _context.FlightDocument_GroupPermissions
                .Where(fdgp => fdgp.FlightDocumentId.Equals(flightDocumentId))
                .Select(fdgp => fdgp.GroupPermissionId)
                .ToList();
            var emailInGroups = _context.Account_GroupPermissions
                .Where(agp => agp.AccountEmail == email && documentGroups.Contains(agp.GroupPermissionId))
                .Any();

            return emailInGroups;
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

            newFD.FlightDocumentIdFK = newFD.FlightDocumentId;
            await _context.SaveChangesAsync();


            foreach (var group in model.GroupPermissionIds)
            {
                var newFD_Group = new FlightDocument_GroupPermission
                {
                    GroupPermissionId = group,
                    FlightDocumentId = (int)newFD.FlightDocumentId
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
                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
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
            if (!user.IsActived)
            {
                return new APIResponse
                {
                    success = false,
                    message = "Your account is locked",
                    msg = "Your account is locked"
                };
            }
            var allD = _context.FlightDocuments
                .Include(dt => dt.DocumentTypeNavigation)
                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                .Include(f => f.FlightNavigation)
                .Where(vp => vp.VersionPatch.Equals(0))
                .AsQueryable();
            if (!user.RoleNavigation.RoleId.Equals(_iConfiguration["Role:admin-id"]) ||
                !user.RoleNavigation.RoleId.Equals(_iConfiguration["Role:owner-id"]))
            {
                allD = allD.Where(a => a.Creator.Equals(user.Email));
            }
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

        public async Task<APIResponse> GetDocumentById(int documentId)
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
            if (user.RoleNavigation.RoleId.Equals(_iConfiguration["Role:pilot-id"]) || user.RoleNavigation.RoleId.Equals(_iConfiguration["Role:flight-attendant-id"]))
            {
                if (!IsEmailInFlightDocumentGroups(user.Email, documentId))
                {
                    return new APIResponse
                    {
                        success = false,
                        message = "Your account in group of documents",
                        msg = "Your account in group of documents"
                    };
                }
                if (!IsEmailInReadOnlyGroups(user.Email))
                {
                    return new APIResponse
                    {
                        success = false,
                        message = "Your account not permission to read",
                        msg = "Your account not permission to read"
                    };
                }
            }
            var doc = await _context.FlightDocuments
                            .Include(dt => dt.DocumentTypeNavigation)
                            .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                            .Include(f => f.FlightNavigation)
                            .Where(d => d.FlightDocumentId.Equals(documentId))
                            .FirstOrDefaultAsync();
            return new APIResponse
            {
                success = true,
                message = "List flight document.",
                msg = "List flight document.",
                data = _mapper.Map<FlightDocumentViewModel>(doc)
            };
        }

        public async Task<APIResponse> GetDocumentUpdatedVersion(int documentId)
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
            var allD = await _context.FlightDocuments
                .Include(dt => dt.DocumentTypeNavigation)
                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                .Include(f => f.FlightNavigation)
                .Where(df => df.FlightDocumentIdFK.Equals(documentId))
                .Where(vp => vp.VersionPatch > 0)
                .ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List flight document.",
                msg = "List flight document.",
                data = _mapper.Map<List<FlightDocumentViewModel>>(allD)
            };
        }

        public async Task<APIResponse> GetLastVersionDocumentByFlightId(int flightId)
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
            var docLastVersion = _context.FlightDocuments
                .Include(dt => dt.DocumentTypeNavigation)
                .Include(f => f.FlightNavigation)
                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                .AsQueryable();

            var docOriginal = await _context.FlightDocuments
                .Where(fd => fd.VersionPatch.Equals(0))
                .Where(fd => fd.FlightId.Equals(flightId))
                .ToListAsync();
            ICollection<int> LastVersionDocList = docOriginal.Select(fd => (int)fd.FlightDocumentIdFK).Distinct().ToList();
            docLastVersion = docLastVersion.Where(a => LastVersionDocList.Contains((int)a.FlightDocumentId));

            var result = await docLastVersion.OrderByDescending(fd => fd.CreatedDate).ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List flight document.",
                msg = "List flight document.",
                data = _mapper.Map<List<FlightDocumentViewModel>>(result)
            };
        }

        public async Task<APIResponse> GetOriginalDocumentByFlightId(int flightId)
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
                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                .Where(f => f.FlightId.Equals(flightId))
                .Where(vp => vp.VersionPatch.Equals(0))
                .OrderByDescending(cd => cd.CreatedDate)
                .ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List flight document.",
                msg = "List flight document.",
                data = _mapper.Map<List<FlightDocumentViewModel>>(docF)
            };
        }

        public async Task<APIResponse> GetUpdateDocumentByFlightId(int flightId)
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
                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                .Where(f => f.FlightId.Equals(flightId))
                .Where(vp => !vp.VersionPatch.Equals(0))
                .OrderByDescending(cd => cd.CreatedDate)
                .ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List flight document.",
                msg = "List flight document.",
                data = _mapper.Map<List<FlightDocumentViewModel>>(docF)
            };
        }

        public async Task<APIResponse> CountFlightDocumenyByFlightId_CMS(int flightId)
        {
            var countD = await _context.FlightDocuments
                .Include(dt => dt.DocumentTypeNavigation)
                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                .Include(f => f.FlightNavigation)
                .Where(fd => fd.FlightId.Equals(flightId))
                .Where(fd => fd.AccountNavigation.RoleNavigation.RoleId.Equals(_iConfiguration["Role:admin-id"]) ||
                        fd.AccountNavigation.RoleNavigation.RoleId.Equals(_iConfiguration["Role:go-staff-id"]))
                .CountAsync();
            return new APIResponse
            {
                success = true,
                message = "Count document from CMS.",
                msg = "Count document from CMS.",
                data = countD
            };
        }

        public async Task<APIResponse> CountFlightDocumenyByFlightId_Mobile(int flightId)
        {
            var countD = await _context.FlightDocuments
                .Include(dt => dt.DocumentTypeNavigation)
                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                .Include(f => f.FlightNavigation)
                .Where(fd => fd.FlightId.Equals(flightId))
                .Where(fd => fd.AccountNavigation.RoleNavigation.RoleId.Equals(_iConfiguration["Role:pilot-id"]) ||
                        fd.AccountNavigation.RoleNavigation.RoleId.Equals(_iConfiguration["Role:flight-attendant-id"]))
                .CountAsync();
            return new APIResponse
            {
                success = true,
                message = "Count document from CMS.",
                msg = "Count document from CMS.",
                data = countD
            };
        }

        public async Task<APIResponse> Update(int documentId, IFormFile file)
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
            if (user.RoleNavigation.RoleId.Equals(_iConfiguration["Role:pilot-id"]) || user.RoleNavigation.RoleId.Equals(_iConfiguration["Role:flight-attendant-id"]))
            {
                if (!IsEmailInFlightDocumentGroups(user.Email, documentId))
                {
                    return new APIResponse
                    {
                        success = false,
                        message = "Your account in group of documents",
                        msg = "Your account in group of documents"
                    };
                }
                if (!IsEmailInReadAndModifyGroups(user.Email))
                {
                    return new APIResponse
                    {
                        success = false,
                        message = "Your account not permission to modify",
                        msg = "Your account not permission to modify"
                    };
                }
            }
            try
            {
                var previousDocument = await _context.FlightDocuments
                            .Include(dt => dt.DocumentTypeNavigation)
                            .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                            .Include(f => f.FlightNavigation)
                            .Where(d => d.FlightDocumentId.Equals(documentId))
                            .FirstOrDefaultAsync();
                var url = await _cloudinaryService.UploadCloudinary(file, _iConfiguration["Cloudinary_folder:flight-doc-folder"]);

                var newDocument = new FlightDocument
                {
                    Version = previousDocument.Version,
                    Note = previousDocument.Note,
                    DocumentTypeId = previousDocument.DocumentTypeId,
                    FlightId = previousDocument.FlightId,
                    Creator = user.Email,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,

                    FileName = previousDocument.FileName,
                    FileType = Path.GetExtension(file.FileName),
                    FileSize = $"{(double)file.Length / (1024 * 1024):0.##} MB",
                    FileUrl = url,
                    FileViewUrl = (Path.GetExtension(file.FileName) == ".mp3" || Path.GetExtension(file.FileName) == ".mp4" || Path.GetExtension(file.FileName) == ".pdf")
                        ? url : String.Concat("https://docs.google.com/viewer?url=", url),
                };
                if (previousDocument.VersionPatch.Equals(0))
                {
                    newDocument.VersionPatch = await _context.FlightDocuments
                    .CountAsync(fd => fd.FlightDocumentIdFK.Equals(previousDocument.FlightDocumentId)) + 1;
                    newDocument.FlightDocumentIdFK = previousDocument.FlightDocumentId;
                }
                else
                {
                    newDocument.VersionPatch = await _context.FlightDocuments
                    .CountAsync(fd => fd.FlightDocumentIdFK.Equals(previousDocument.FlightDocumentIdFK)) + 1;
                    newDocument.FlightDocumentIdFK = previousDocument.FlightDocumentIdFK;
                }
                await _context.AddAsync(newDocument);
                await _context.SaveChangesAsync();

                if (!previousDocument.VersionPatch.Equals(0))
                {
                    var originalDoc = await _context.FlightDocuments
                                .Include(dt => dt.DocumentTypeNavigation)
                                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                                .Include(f => f.FlightNavigation)
                                .Where(d => d.FlightDocumentId.Equals(previousDocument.FlightDocumentIdFK))
                                .FirstOrDefaultAsync();
                    originalDoc.FlightDocumentIdFK = newDocument.FlightDocumentId;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var originalDoc = await _context.FlightDocuments
                                .Include(dt => dt.DocumentTypeNavigation)
                                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                                .Include(f => f.FlightNavigation)
                                .Where(d => d.FlightDocumentId.Equals(previousDocument.FlightDocumentId))
                                .FirstOrDefaultAsync();
                    originalDoc.FlightDocumentIdFK = newDocument.FlightDocumentId;
                    await _context.SaveChangesAsync();
                }



                var documentGroups = await _context.FlightDocument_GroupPermissions
                .Where(fdgp => fdgp.FlightDocumentId.Equals(previousDocument.FlightDocumentId))
                .Select(fdgp => fdgp.GroupPermissionId)
                .ToListAsync();
                foreach (var group in documentGroups)
                {
                    var newFD_Group = new FlightDocument_GroupPermission
                    {
                        GroupPermissionId = group,
                        FlightDocumentId = (int)newDocument.FlightDocumentId
                    };
                    await _context.AddAsync(newFD_Group);
                };
                await _context.SaveChangesAsync();

                await _iFDRepository.CreateNew($"{user.RoleNavigation.RoleName}: {user.Name}, địa chỉ hộp thư {user.Email} đã chỉnh sửa " +
               $"tài liệu {previousDocument.FileName}.{previousDocument.FileType} v{previousDocument.Version}.{previousDocument.VersionPatch} " +
               $"của chuyến bay {previousDocument.FlightNavigation.FlightCode}{previousDocument.FlightNavigation.FlightId.ToString("D3")}. " +
               $"phiên bản hiện tại của tài liệu: v{newDocument.Version}.{newDocument.VersionPatch}");
            }
            catch
            {
                return new APIResponse
                {
                    success = false,
                    message = "Flight document update fail.",
                    msg = "Flight document update fail."
                };
            }
            return new APIResponse
            {
                success = true,
                message = "Flight document update.",
                msg = "Flight document update."
            };
        }

        public async Task<APIResponse> GetUpdateFlightDocumentByAccount_Mobile(int flightId)
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
            var allD = await _context.FlightDocuments
                .Include(dt => dt.DocumentTypeNavigation)
                .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                .Include(f => f.FlightNavigation)
                .Where(a => a.Creator.Equals(user.Email))
                .Where(a => a.FlightId.Equals(flightId))
                .Where(vp => !vp.VersionPatch.Equals(0))
                .OrderByDescending(d => d.CreatedDate)
                .ToListAsync();
            return new APIResponse
            {
                success = true,
                message = "List flight document.",
                msg = "List flight document.",
                data = _mapper.Map<List<FlightDocumentViewModel>>(allD)
            };
        }

        public async Task<List<int>> GetListDocumentId(int flightId)
        {
            var listDocumentId = await _context.FlightDocuments
                    .Include(dt => dt.DocumentTypeNavigation)
                    .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                    .Include(f => f.FlightNavigation)
                    .Where(f => f.FlightId.Equals(flightId))
                    .Select(f => (int) f.FlightDocumentId)
                    .ToListAsync();
            return listDocumentId;
        }
    }
}
