using AutoMapper;
using Entities;
using Entities.DTO;
using Entities.Models;

namespace ParkingApp2Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserLite>();

            CreateMap<WebHookSubscription, WebHookSubscriptionDto>();
            CreateMap<WebHookSubscriptionForManipulationDto, WebHookSubscription>();
        }
    }
}