using AutoMapper;
using VKVideoReviews.BL.Services.AppAuth.Models;

namespace VKVideoReviews.WebApi.Mapper;

public class AppAuthWebApiProfile : Profile
{
    public AppAuthWebApiProfile()
    {
        CreateMap<AuthTokensResult, AuthTokensResult>();
    }
}