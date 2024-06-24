using AutoMapper;
using WalksNZAPI.Models.Domain;
using WalksNZAPI.Models.DTO;

namespace WalksNZAPI.Profiles
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
        }
    }
}
