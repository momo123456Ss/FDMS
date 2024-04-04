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
        }
    }
}
