using AutoMapper;
using VKVideoReviews.BL.Services.Favorite.Models;
using VKVideoReviews.BL.Services.Reviews.Models;
using VKVideoReviews.WebApi.Controllers.Requests.Favorite;
using VKVideoReviews.WebApi.Controllers.Responses.Favorite;

namespace VKVideoReviews.WebApi.Mapper;

public class FavoriteWebApiProfile : Profile
{
    public FavoriteWebApiProfile()
    {
        CreateMap<CreateFavoriteRequest, CreateFavoriteModel>();
        CreateMap<FavoriteModel, FavoriteResponse>();
    }
}