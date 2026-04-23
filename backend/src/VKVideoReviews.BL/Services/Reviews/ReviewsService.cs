using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using VKVideoReviews.BL.Common.Pagination;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.Reviews.Interfaces;
using VKVideoReviews.BL.Services.Reviews.Models;
using VKVideoReviews.BL.Services.Videos;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.Reviews;

public class ReviewsService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateReviewModel> createValidator,
    IValidator<UpdateReviewModel> updateValidator,
    IValidator<PageRequestModel> pageRequestValidator,
    IDistributedCache cache) : IReviewsService
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
            await cache.RemoveAsync(VideosService.VideoCacheKey(videoId));
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
            await cache.RemoveAsync(VideosService.VideoCacheKey(videoId));
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

    public async Task<PagedListModel<ReviewModel>> GetAllReviewsPagedAsync(PageRequestModel pageRequest)
    {
        await ValidateAsync(pageRequestValidator, pageRequest);

        var (reviews, totalCount) = await unitOfWork.Reviews
            .GetReviewsPagedWithUsersAndVideosAsync(pageRequest.PageNumber, pageRequest.PageSize);

        var items = mapper.Map<List<ReviewModel>>(reviews);
        return new PagedListModel<ReviewModel>(items, totalCount, pageRequest.PageNumber, pageRequest.PageSize);
    }

    public async Task<PagedListModel<ReviewModel>> GetReviewsByVideoPagedAsync(Guid videoId,
        PageRequestModel pageRequest)
    {
        await ValidateAsync(pageRequestValidator, pageRequest);

        var video = await unitOfWork.Videos.GetVideoByIdAsync(videoId);
        if (video is null)
            throw new NotFoundException("Video", videoId);

        var (reviews, totalCount) = await unitOfWork.Reviews
            .GetReviewsByVideoPagedWithUsersAsync(videoId, pageRequest.PageNumber, pageRequest.PageSize);

        var items = mapper.Map<List<ReviewModel>>(reviews);
        return new PagedListModel<ReviewModel>(items, totalCount, pageRequest.PageNumber, pageRequest.PageSize);
    }

    public async Task<PagedListModel<ReviewModel>> GetMyReviewsPagedAsync(Guid userId, PageRequestModel pageRequest)
    {
        await ValidateAsync(pageRequestValidator, pageRequest);

        var (reviews, totalCount) = await unitOfWork.Reviews
            .GetReviewsByUserPagedWithVideosAsync(userId, pageRequest.PageNumber, pageRequest.PageSize);

        var items = mapper.Map<List<ReviewModel>>(reviews);
        return new PagedListModel<ReviewModel>(items, totalCount, pageRequest.PageNumber, pageRequest.PageSize);
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
            await cache.RemoveAsync(VideosService.VideoCacheKey(videoId));
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