using AutoMapper;
using VKVideoReviews.BL.Services.VideoTypes.Models;
using VKVideoReviews.WebApi.Controllers.Requests.VideoType;

namespace VKVideoReviews.WebApi.Mapper;

public class VideoTypesWebApiProfile : Profile
{
    public VideoTypesWebApiProfile()
    {
        CreateMap<CreateVideoTypeRequest, CreateVideoTypeModel>();
        CreateMap<UpdateVideoTypeRequest, UpdateVideoTypeModel>();
    }
}