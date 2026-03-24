using AutoMapper;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Mapper;

public class UsersBLProfile : Profile
{
    public UsersBLProfile()
    {
        CreateMap<VkApiUserResponse, UserEntity>()
            .ForMember(dest => dest.AvatarUrl, opt
                => opt.MapFrom(src => src.AvatarUrl))
            .ForMember(dest => dest.Name, opt
                => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt
                => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.VkUserId, opt
                => opt.MapFrom(src => src.VkUserId));
    }
}