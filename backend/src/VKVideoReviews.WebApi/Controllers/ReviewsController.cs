using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.Reviews.Interfaces;
using VKVideoReviews.BL.Services.Reviews.Models;
using VKVideoReviews.WebApi.Controllers.Helpers;
using VKVideoReviews.WebApi.Controllers.Requests.Reviews;
using VKVideoReviews.WebApi.Controllers.Responses.Reviews;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController(
    IReviewsService reviewsService,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{reviewId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ReviewResponse>> GetReview(Guid reviewId)
    {
        var reviewModel = await reviewsService.GetReviewAsync(reviewId);
        return Ok(mapper.Map<ReviewResponse>(reviewModel));
    }

    [HttpGet("")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReviewsListResponse>> GetAllReviews()
    {
        var reviewModels = await reviewsService.GetAllReviewAsync();
        return Ok(new ReviewsListResponse(mapper.Map<List<ReviewResponse>>(reviewModels)));
    }


    [HttpPost("videos/{videoId}")]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<ReviewResponse>> CreateMyReview(
        Guid videoId,
        [FromBody] CreateReviewRequest createReviewRequest)
    {
        var userId = this.GetCurrentUserId();

        var createReviewModel = mapper.Map<CreateReviewModel>(createReviewRequest);
        var reviewModel = await reviewsService.CreateReviewAsync(userId, videoId, createReviewModel);

        return Ok(mapper.Map<ReviewResponse>(reviewModel));
    }

    [HttpDelete("videos/{videoId}")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> DeleteMyReview(Guid videoId)
    {
        var userId = this.GetCurrentUserId();

        await reviewsService.DeleteReviewAsync(userId, videoId);

        return NoContent();
    }

    [HttpPut("videos/{videoId}")]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<ReviewResponse>> UpdateMyReview(
        Guid videoId,
        [FromBody] UpdateReviewRequest updateReviewRequest)
    {
        var userId = this.GetCurrentUserId();

        var updateReviewModel = mapper.Map<UpdateReviewModel>(updateReviewRequest);
        var reviewModel = await reviewsService.UpdateReviewAsync(userId, videoId, updateReviewModel);

        return Ok(mapper.Map<ReviewResponse>(reviewModel));
    }
}