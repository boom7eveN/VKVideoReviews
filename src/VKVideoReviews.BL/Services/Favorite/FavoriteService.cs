using AutoMapper;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.Favorite.Interfaces;
using VKVideoReviews.BL.Services.Favorite.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.Favorite;

public class FavoriteService(IMapper mapper, IUnitOfWork unitOfWork) : IFavoriteService
{
    public async Task<FavoriteModel> CreateFavoriteAsync(Guid userId, CreateFavoriteModel model)
    {
        var favoriteEntity = mapper.Map<FavoriteEntity>(model);
        favoriteEntity.UserId = userId;
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var video = await unitOfWork.Videos.GetVideoByIdWithGenresAndVideotypesAsync(model.VideoId);
            if (video == null)
                throw new NotFoundException("Video", model.VideoId);

            favoriteEntity = await unitOfWork.Favorite.CreateFavoriteAsync(favoriteEntity);
            if (favoriteEntity is null)
                throw new AlreadyExistsException("Favorite");
            await unitOfWork.CommitAsync();
            favoriteEntity = await unitOfWork.Favorite.GetFavoriteWithVideoAsync(userId, model.VideoId);
            return mapper.Map<FavoriteModel>(favoriteEntity);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<FavoriteModel>> GetAllFavoriteAsync(Guid userId)
    {
        var favorite = await unitOfWork.Favorite.GetAllFavoriteWithVideoAsync(userId);
        return mapper.Map<IEnumerable<FavoriteModel>>(favorite);
    }

    public async Task DeleteFavoriteAsync(Guid userId, Guid videoId)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var favorite = await unitOfWork.Favorite.GetFavoriteAsync(userId, videoId);
            if (favorite is null)
                throw new NotFoundException("Favorite");
            unitOfWork.Favorite.DeleteFavorite(favorite);
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}