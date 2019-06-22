using AutoMapper;
using Postcore.AdApi.Infrastructure.Models;
using Postcore.AdApi.Shared.Models;

namespace Postcore.AdApi.Services
{
    public class AdProfile : Profile
    {
        public AdProfile()
        {
            CreateMap<AdData, AdDto>().ReverseMap();
        }
    }
}
