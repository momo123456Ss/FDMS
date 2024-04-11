using AutoMapper;
using FDMS.Entity;
using FDMS.Model;

namespace FDMS.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            //Account
            #region
            CreateMap<Account, AccountCreateModel>().ReverseMap();
            CreateMap<Account, AccountUpdateModel>().ReverseMap();
            CreateMap<Account, AccountViewModel>()
                .ForMember(dest => dest.RoleNavigation, opt => opt.MapFrom(src => src.RoleNavigation))
                .ReverseMap();
            #endregion
            //General
            #region
            CreateMap<General, GeneralUpdateModel>().ReverseMap();
            CreateMap<General, GeneralViewModel>().ReverseMap();
            #endregion
            //Role
            #region
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<Role, RoleCreateOrUpdateModel>().ReverseMap();
            #endregion
            //GroupPermission
            #region
            CreateMap<GroupPermission, GroupPermissionCreateOrUpdateModel>().ReverseMap();
            CreateMap<GroupPermission, GroupPermissionViewModel>()
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.TotalMembers + " " + "accounts"))
                .ForMember(dest => dest.AccountNavigation, opt => opt.MapFrom(src => src.AccountNavigation))
                .ReverseMap();
            #endregion
            //SystemNoficationViewModel
            #region
            CreateMap<SystemNofication, SystemNoficationViewModel>().ReverseMap();
            #endregion
            //Flight
            #region
            CreateMap<Flight, FlightCreateModel>().ReverseMap();
            CreateMap<Flight, FlightConfirmModel>().ReverseMap();
            CreateMap<Flight, FilghtViewModel>()
                .ForMember(dest => dest.FlightNo, opt => opt.MapFrom(src => src.FlightCode + src.FlightId.ToString("D3")))
                .ForMember(dest => dest.TotalDocuments, opt => opt.MapFrom(src => src.TotalDocuments.ToString("00")))
                .ReverseMap();
            #endregion
            //DocumentType
            #region
            CreateMap<DocumentType, DocumentTypeCreateOrUpdateModel>().ReverseMap();
            CreateMap<DocumentType, DocumentTypeViewModel>()
                .ForMember(dest => dest.AccountNavigation, opt => opt.MapFrom(src => src.AccountNavigation))
                .ReverseMap();
            CreateMap<DocumentType_Permission, DocumentType_GroupPermissionViewModel>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GroupPermissionNavigation.GroupName))
                .ReverseMap();

            #endregion
            //Flight doc
            CreateMap<FlightDocument, FlightDocumentCreateModel>().ReverseMap();
            CreateMap<FlightDocument, FlightDocumentViewModel>()
                .ForMember(dest => dest.DocumentTypeNavigation, opt => opt.MapFrom(src => src.DocumentTypeNavigation))
                .ForMember(dest => dest.VersionToString, opt => opt.MapFrom(src => String.Concat($"v{src.Version}.{src.VersionPatch}")))
                .ForMember(dest => dest.AccountNavigation, opt => opt.MapFrom(src => src.AccountNavigation))
                .ForMember(dest => dest.FlightNo, opt => opt.MapFrom(src => src.FlightNavigation.FlightCode + src.FlightNavigation.FlightId.ToString("D3")))
                .ReverseMap();

            //FDHistory
            #region
            CreateMap<FDHistory, FDHistoryViewModel>().ReverseMap();
            #endregion
        }
    }
}
