using AutoMapper;
using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.BL.Services.VideoTypes.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Mapper;

public class VideoTypesBLProfile : Profile
{
    public VideoTypesBLProfile()
    {
        CreateMap<CreateVideoTypeModel, VideoTypeEntity>()
            .ForMember(dest => dest.VideoTypeId, opt
                => opt.Ignore())
            .ForMember(dest => dest.Title, opt
                => opt.MapFrom(src => src.Title));

        CreateMap<UpdateVideoTypeModel, VideoTypeEntity>()
            .ForMember(dest => dest.VideoTypeId, opt
                => opt.Ignore())
            .ForMember(dest => dest.Title, opt
                => opt.MapFrom(src => src.Title));

        CreateMap<VideoTypeEntity, VideoTypeModel>()
            .ForMember(dest => dest.Id, opt
                => opt.MapFrom(src => src.VideoTypeId))
            .ForMember(dest => dest.Title, opt
                => opt.MapFrom(src => src.Title));
    }
}