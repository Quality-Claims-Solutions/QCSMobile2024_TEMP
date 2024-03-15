using AutoMapper;
using QCSMobile2024.Shared.Models.CustomModels;
using QCSMobile2024.Shared.Models.EntityModels;
using QCSMobile2024.Shared.Models.ViewModels;

namespace QCSMobile2024.Shared.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FnolViewModel, Fnol>();

            CreateMap<FnolViewModel, PhotosExpress>()
                .ForMember(dest => dest.VehicleOwner_FName, opt => opt.MapFrom(src => src.ClaimantFirstName != null ? src.ClaimantFirstName : src.InsuredFirstName))
                .ForMember(dest => dest.VehicleOwner_LName, opt => opt.MapFrom(src => src.ClaimantLastName != null ? src.ClaimantLastName : src.InsuredLastName))
                .ForMember(dest => dest.VehicleOwner_Email, opt => opt.MapFrom(src => src.ClaimantEmail != null ? src.ClaimantEmail : src.InsuredPrimaryEmail))
                .ForMember(dest => dest.VehicleOwner_MobilePhone, opt => opt.MapFrom(src => src.ClaimantPhone != null ? src.ClaimantPhone : src.InsuredPrimaryPhone))
                .ForMember(dest => dest.Vehicle_Make, opt => opt.MapFrom(src => src.InsuredVehicleMake))
                .ForMember(dest => dest.Vehicle_Model, opt => opt.MapFrom(src => src.InsuredVehicleModel))
                .ForMember(dest => dest.Vehicle_Year, opt => opt.MapFrom(src => src.InsuredVehicleYear))
                .ForMember(dest => dest.Vehicle_VIN, opt => opt.MapFrom(src => src.InsuredVehicleVin))
                .ForMember(dest => dest.ClaimNumber, opt => opt.MapFrom(src => src.InsuredVehicleVin)) //temporary
                .ForMember(dest => dest.InsuranceCompanyName, opt => opt.MapFrom(src => "Temporary")) //temporary
                .ForMember(dest => dest.DateEntered, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<FileAttachmentViewModel, PhotosExpress_Attachment>()
                .ForMember(dest => dest.PhotosExpressID, opt => opt.MapFrom(src => src.PhotosExpressId))
                .ForMember(dest => dest.AddedBy, opt => opt.MapFrom(src => "Owner"));

            CreateMap<Fnol_Attachments, FileAttachmentViewModel>()
                .ForMember(dest => dest.FnolId, opt => opt.MapFrom(src => src.FnolID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Description));

            CreateMap<FileAttachmentViewModel, Fnol_Attachments>()
                .ForMember(dest => dest.FnolID, opt => opt.MapFrom(src => src.FnolId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Title));

            CreateMap<FileAttachmentViewModel, UneditedContactInformation>();
        }
    }
}
