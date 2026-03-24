using AutoMapper;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.WebApi.Controllers.Responses.AppAuth;

namespace VKVideoReviews.WebApi.Mapper;

public class AppAuthWebApiProfile : Profile
{
    public AppAuthWebApiProfile()
    {
        CreateMap<AuthTokensResult, AuthTokensResponse>();
    }
}