using AutoMapper;
using VKVideoReviews.BL.Services.Favorite.Interfaces;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.Favorite;

public class FavoriteService(
    IMapper mapper,
    IAuthUnitOfWork unitOfWork) : IFavoriteService
{
}