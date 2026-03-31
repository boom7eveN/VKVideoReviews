using AutoMapper;
using VKVideoReviews.BL.Services.Videos.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Mapper;

public class VideosBLProfile : Profile
{
    public VideosBLProfile()
    {
        CreateMap<CreateVideoModel, VideoEntity>();
        CreateMap<VideoEntity, VideoModel>()
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.GenresVideos.Select(gv => gv.Genre)));
    }
}