using AutoMapper;
using VKVideoReviews.BL.Services.Reviews.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Mapper;

public class ReviewsBLProfile : Profile
{
    public ReviewsBLProfile()
    {
        CreateMap<CreateReviewModel, ReviewEntity>();
        CreateMap<ReviewEntity, ReviewModel>();
    }
}