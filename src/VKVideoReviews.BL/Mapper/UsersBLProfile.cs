using AutoMapper;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Mapper;

public class UsersBLProfile : Profile
{
    public UsersBLProfile()
    {
        CreateMap<VkApiUserResponse, UserEntity>();
    }
}