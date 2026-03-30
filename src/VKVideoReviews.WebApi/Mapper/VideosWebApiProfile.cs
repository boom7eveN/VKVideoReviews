using AutoMapper;
using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.BL.Services.Videos;
using VKVideoReviews.BL.Services.Videos.Models;
using VKVideoReviews.WebApi.Controllers.Requests.Videos;


namespace VKVideoReviews.WebApi.Mapper;


public class VideosWebApiProfile : Profile
{
    public VideosWebApiProfile()
    {
        CreateMap<CreateVideoRequest, CreateVideoModel>();
    }
}