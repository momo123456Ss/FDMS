using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using FDMS.Entity;
using FDMS.Model;
using FDMS.Service.CloudinaryService;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using AutoMapper;
using FDMS.Helper;
using FDMS.Repository.SystemNoficationRepository;
using FDMS.Service.JWTService;

namespace FDMS.Repository.GeneralRepository
{
    public class GeneralRepository : IGeneralRepository
    {
        private readonly FDMSContext _context;
        private readonly IJWTService _jWTService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _iConfiguration;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ISystemNoficationRepository _iSystemNoficationRepository;

        public GeneralRepository(FDMSContext context, ICloudinaryService cloudinaryService, IMapper mapper
            , IConfiguration iConfiguration, ISystemNoficationRepository iSystemNoficationRepository, IJWTService jWTService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _iConfiguration = iConfiguration;
            _iSystemNoficationRepository = iSystemNoficationRepository;
            _jWTService = jWTService;
        }
        public async Task<APIResponse> EditGeneral(GeneralUpdateModel model)
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
            var general = await _context.Generals.FirstOrDefaultAsync(g => g.GeneralId.Equals(_iConfiguration["General:general-id"]));
            if (model.LogoFile != null)
            {
                if (await _cloudinaryService.DeleteCloudinary(general.Logo, _iConfiguration["Cloudinary_folder:logo-folder"]))
                {                  
                    general.Logo = await _cloudinaryService.UploadCloudinary(model.LogoFile, _iConfiguration["Cloudinary_folder:logo-folder"]);
                }
            }
            foreach (var property in typeof(GeneralUpdateModel).GetProperties())
            {
                var modelValue = property.GetValue(model);
                if (modelValue != null)
                {
                    var generalProperty = typeof(General).GetProperty(property.Name);
                    if (generalProperty != null)
                    {
                        generalProperty.SetValue(general, modelValue);
                    }
                }
            }
            await _context.SaveChangesAsync();
            await _iSystemNoficationRepository.CreateNew($"Tài khoản có tên {user.Name}, địa chỉ hộp thư {user.Email} đã thay đổi tùy chỉnh General.");
            return new APIResponse
            {
                success = true,
                message = "Update general success.",
                msg = "Update general success."
            };
        }

        public async Task<APIResponse> GetGeneral()
        {
            var general = await _context.Generals.FirstOrDefaultAsync(g => g.GeneralId.Equals(_iConfiguration["General:general-id"]));
            return new APIResponse
            {
                success = true,
                message = "General",
                msg = "General",
                data = _mapper.Map<GeneralViewModel>(general)
            };
        }
    }
}
