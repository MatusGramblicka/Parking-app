using AutoMapper;
using Entities;
using Entities.DTO;

namespace ParkingApp2Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserLite>();
        }
    }
}
