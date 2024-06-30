using AutoMapper;
using WalksNZAPI.Models.Domain;
using WalksNZAPI.Models.DTO;

namespace WalksNZAPI.Profiles
{
    public class WalkProfile : Profile
    {
        public WalkProfile()
        {
            CreateMap<Walk,WalkDTO>().ReverseMap();
            CreateMap<WalkDifficulty, WalkDifficultyDTO>().ReverseMap();
        }
    }
}
