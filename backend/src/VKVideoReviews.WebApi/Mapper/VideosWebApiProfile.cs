using AutoMapper;
using VKVideoReviews.BL.Services.Videos.Models;
using VKVideoReviews.WebApi.Controllers.Requests.Videos;
using VKVideoReviews.WebApi.Controllers.Responses.Videos;

namespace VKVideoReviews.WebApi.Mapper;

public class VideosWebApiProfile : Profile
{
    public VideosWebApiProfile()
    {
        CreateMap<CreateVideoRequest, CreateVideoModel>();
        CreateMap<UpdateVideoRequest, UpdateVideoModel>();
        CreateMap<VideoModel, VideoResponse>();
    }
}