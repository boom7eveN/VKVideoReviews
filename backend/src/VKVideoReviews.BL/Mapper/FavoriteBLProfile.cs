using AutoMapper;
using VKVideoReviews.BL.Services.Favorite.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Mapper;

public class FavoriteBLProfile : Profile
{
    public FavoriteBLProfile()
    {
        CreateMap<CreateFavoriteModel, FavoriteEntity>();
        CreateMap<FavoriteEntity, FavoriteModel>();
    }
}