using AutoMapper;
using BillingApp.Models; // Assuming User is here
using BillingApp.DTO;   // Assuming UserDTO is here

namespace BillingApp.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                // Add other mappings as needed based on UserDTO properties
                .ForMember(dest => dest.Role, opt => opt.Ignore()) // Populated manually in PopulateUserRolesAndAdminInfo
                .ForMember(dest => dest.AdminId, opt => opt.Ignore()); // Populated manually
        }
    }
}