using AutoMapper;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.Reviews.Interfaces;
using VKVideoReviews.BL.Services.Reviews.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.Reviews;

public class ReviewsService(IUnitOfWork unitOfWork, IMapper mapper) : IReviewsService
{
    public async Task<ReviewModel> CreateReviewAsync(Guid userId, Guid videoId, CreateReviewModel model)
    {
        var review = mapper.Map<ReviewEntity>(model);
        review.ReviewId = Guid.NewGuid();
        var currentTime =  DateTime.UtcNow;
        review.CreateDate = currentTime;
        review.UpdateDate = currentTime;

        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var lockedVideo = await unitOfWork.Videos.LockForUpdateAsync(videoId);
            if (lockedVideo == null)
                throw new NotFoundException("Video", videoId);

            var user = await unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User", userId);
            review.UserId = userId;

            var existingReview = await unitOfWork.Reviews
                .GetByUserAndVideoIdsWithUserAndVideoAsync(userId, videoId);
            if (existingReview != null)
                throw new AlreadyExistsException("Review for this video from this user");

            review.VideoId = videoId;
            review = await unitOfWork.Reviews.CreateAsync(review);
            if (review is null)
                throw new AlreadyExistsException("Review for this video from this user");

            await unitOfWork.SaveChangesAsync();

            await unitOfWork.Videos.UpdateVideoRatingAsync(videoId);

            await transaction.CommitAsync();

            var createdReview = await unitOfWork.Reviews.GetByReviewIdWithUserAndVideoAsync(review.ReviewId);
            return mapper.Map<ReviewModel>(createdReview);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteReviewAsync(Guid userId, Guid videoId)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var lockedVideo = await unitOfWork.Videos.LockForUpdateAsync(videoId);
            if (lockedVideo == null)
                throw new NotFoundException("Video", videoId);

            var review = await unitOfWork.Reviews.GetByUserAndVideoIdsWithUserAndVideoAsync(userId, videoId);
            if (review is null)
                throw new NotFoundException("Review");

            unitOfWork.Reviews.Delete(review);
            await unitOfWork.SaveChangesAsync();

            await unitOfWork.Videos.UpdateVideoRatingAsync(videoId);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ReviewModel> GetReviewAsync(Guid reviewId)
    {
        var review = await unitOfWork.Reviews.GetByReviewIdWithUserAndVideoAsync(reviewId);
        return mapper.Map<ReviewModel>(review);
    }

    public async Task<IEnumerable<ReviewModel>> GetAllReviewAsync()
    {
        var reviews = await unitOfWork.Reviews.GetAllWithUserAndVideoAsync();
        return mapper.Map<IEnumerable<ReviewModel>>(reviews);
    }

    public async Task<ReviewModel> UpdateReviewAsync(Guid userId, Guid videoId, UpdateReviewModel model)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var lockedVideo = await unitOfWork.Videos.LockForUpdateAsync(videoId);
            if (lockedVideo == null)
                throw new NotFoundException("Video", videoId);

            var review = await unitOfWork.Reviews.GetByUserAndVideoIdsWithUserAndVideoAsync(userId, videoId);
            if (review is null)
                throw new NotFoundException("Review");

            review.Text = model.Text;
            review.Rate = model.Rate;
            review.UpdateDate = DateTime.UtcNow;

            unitOfWork.Reviews.Update(review);
            await unitOfWork.SaveChangesAsync();

            await unitOfWork.Videos.UpdateVideoRatingAsync(videoId);

            await transaction.CommitAsync();

            var updatedReview = await unitOfWork.Reviews.GetByReviewIdWithUserAndVideoAsync(review.ReviewId);
            return mapper.Map<ReviewModel>(updatedReview);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}