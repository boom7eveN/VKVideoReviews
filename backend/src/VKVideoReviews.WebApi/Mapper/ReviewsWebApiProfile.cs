using AutoMapper;
using VKVideoReviews.BL.Services.Reviews.Models;
using VKVideoReviews.WebApi.Controllers.Requests.Reviews;
using VKVideoReviews.WebApi.Controllers.Responses.Reviews;

namespace VKVideoReviews.WebApi.Mapper;

public class ReviewsWebApiProfile : Profile
{
    public ReviewsWebApiProfile()
    {
        CreateMap<CreateReviewRequest, CreateReviewModel>();
        CreateMap<UpdateReviewRequest, UpdateReviewModel>();
        CreateMap<ReviewModel, ReviewResponse>();
    }
}