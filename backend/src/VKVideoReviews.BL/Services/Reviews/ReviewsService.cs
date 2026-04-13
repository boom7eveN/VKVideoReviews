using AutoMapper;
using FluentValidation;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.Reviews.Interfaces;
using VKVideoReviews.BL.Services.Reviews.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.Reviews;

public class ReviewsService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    IValidator<CreateReviewModel> createValidator,
    IValidator<UpdateReviewModel> updateValidator) : IReviewsService
{
    public async Task<ReviewModel> CreateReviewAsync(Guid userId, Guid videoId, CreateReviewModel createReviewModel)
    {
        await ValidateAsync(createValidator, createReviewModel);
        
        var review = mapper.Map<ReviewEntity>(createReviewModel);
        review.ReviewId = Guid.NewGuid();
        var currentTime = DateTime.UtcNow;
        review.CreateDate = currentTime;
        review.UpdateDate = currentTime;
        review.UserId = userId;
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var lockedVideo = await unitOfWork.Videos.GetVideoByIdLockForUpdateAsync(videoId);
            if (lockedVideo == null)
                throw new NotFoundException("Video", videoId);
            review.VideoId = videoId;

            var existingReview = await unitOfWork.Reviews
                .GetReviewByUserAndVideoIdsAsync(userId, videoId);
            if (existingReview != null)
                throw new AlreadyExistsException("Review for this video from this user");

            review = await unitOfWork.Reviews.CreateReviewAsync(review);

            await unitOfWork.SaveChangesAsync();

            await unitOfWork.Videos.UpdateVideoRatingByIdAsync(videoId);

            await transaction.CommitAsync();

            var createdReview = await unitOfWork.Reviews.GetReviewByIdWithUserAndVideoAsync(review.ReviewId);
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
            var lockedVideo = await unitOfWork.Videos.GetVideoByIdLockForUpdateAsync(videoId);
            if (lockedVideo == null)
                throw new NotFoundException("Video", videoId);

            var review = await unitOfWork.Reviews.GetReviewByUserAndVideoIdsAsync(userId, videoId);
            if (review is null)
                throw new NotFoundException("Review");

            unitOfWork.Reviews.DeleteReview(review);
            await unitOfWork.SaveChangesAsync();

            await unitOfWork.Videos.UpdateVideoRatingByIdAsync(videoId);

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
        var review = await unitOfWork.Reviews.GetReviewByIdWithUserAndVideoAsync(reviewId);
        if (review is null)
            throw new NotFoundException("Review", reviewId);
        return mapper.Map<ReviewModel>(review);
    }

    public async Task<IEnumerable<ReviewModel>> GetAllReviewAsync()
    {
        var reviews = await unitOfWork.Reviews.GetAllReviewsWithUsersAndVideosAsync();
        return mapper.Map<IEnumerable<ReviewModel>>(reviews);
    }

    public async Task<ReviewModel> UpdateReviewAsync(Guid userId, Guid videoId, UpdateReviewModel updateReviewModel)
    {
        await ValidateAsync(updateValidator, updateReviewModel);
        
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var lockedVideo = await unitOfWork.Videos.GetVideoByIdLockForUpdateAsync(videoId);
            if (lockedVideo == null)
                throw new NotFoundException("Video", videoId);

            var review = await unitOfWork.Reviews.GetReviewByUserAndVideoIdsAsync(userId, videoId);
            if (review is null)
                throw new NotFoundException("Review");

            review.Text = updateReviewModel.Text;
            review.Rate = updateReviewModel.Rate;
            review.UpdateDate = DateTime.UtcNow;

            unitOfWork.Reviews.UpdateReview(review);
            await unitOfWork.SaveChangesAsync();

            await unitOfWork.Videos.UpdateVideoRatingByIdAsync(videoId);

            await transaction.CommitAsync();

            var updatedReview = await unitOfWork.Reviews.GetReviewByIdWithUserAndVideoAsync(review.ReviewId);
            return mapper.Map<ReviewModel>(updatedReview);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    private static async Task ValidateAsync<T>(IValidator<T> validator, T model)
    {
        var validationResult = await validator.ValidateAsync(model);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            
            throw new ModelValidationException(errors);
        }
    }
}