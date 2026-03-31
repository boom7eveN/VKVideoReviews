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
[Route("[controller]")]
public class ReviewsController(
    ILogger<ReviewsController> logger,
    IReviewsService reviewsService,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ReviewResponse>> GetReview(Guid id)
    {
        var review = await reviewsService.GetReviewAsync(id);
        return Ok(mapper.Map<ReviewResponse>(review));
    }

    [HttpGet("")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReviewsListResponse>> GetAllReviews()
    {
        var reviews = await reviewsService.GetAllReviewAsync();
        return Ok(new ReviewsListResponse(mapper.Map<List<ReviewResponse>>(reviews)));
    }


    [HttpPost("videos/{videoId:guid}")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<ReviewResponse>> CreateReview(
        Guid videoId,
        [FromBody] CreateReviewRequest request)
    {
        var userId = this.GetCurrentUserId();

        var createReviewModel = mapper.Map<CreateReviewModel>(request);
        var review = await reviewsService.CreateReviewAsync(userId, videoId, createReviewModel);

        return CreatedAtAction(
            nameof(GetReview),
            new { id = review.ReviewId },
            mapper.Map<ReviewResponse>(review));
    }

    [HttpDelete("videos/{videoId:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteMyReview(Guid videoId)
    {
        var userId = this.GetCurrentUserId();

        await reviewsService.DeleteReviewAsync(userId, videoId);

        return NoContent();
    }

    [HttpPut("videos/{videoId:guid}")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<ReviewResponse>> UpdateReview(
        Guid videoId,
        [FromBody] UpdateReviewRequest request)
    {
        var userId = this.GetCurrentUserId();

        var updateReviewModel = mapper.Map<UpdateReviewModel>(request);
        var review = await reviewsService.UpdateReviewAsync(userId, videoId, updateReviewModel);

        return Ok(mapper.Map<ReviewResponse>(review));
    }
}