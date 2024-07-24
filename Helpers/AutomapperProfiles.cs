using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Model;

namespace DatingApp.Helpers
{
    public class AutomapperProfiles:Profile
    {
        public AutomapperProfiles() {
            CreateMap<UserDetails, MerberDto>(); 
            CreateMap<UserDetails, PhotoDto>();
            CreateMap<MemberUpdateDto, UserDetails>();
        }
    }
}
