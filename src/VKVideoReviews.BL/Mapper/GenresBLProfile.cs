using AutoMapper;
using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Mapper;

public class GenresBLProfile : Profile
{
    public GenresBLProfile()
    {
        CreateMap<CreateGenreModel, GenreEntity>()
            .ForMember(dest => dest.GenreId, opt
                => opt.Ignore())
            .ForMember(dest => dest.Title, opt
                => opt.MapFrom(src => src.Title));
        
        CreateMap<UpdateGenreModel, GenreEntity>()
            .ForMember(dest => dest.GenreId, opt
                => opt.Ignore())
            .ForMember(dest => dest.Title, opt
                => opt.MapFrom(src => src.Title));

        CreateMap<GenreEntity, GenreModel>()
            .ForMember(dest => dest.Id, opt
                => opt.MapFrom(src => src.GenreId))
            .ForMember(dest => dest.Title, opt
                => opt.MapFrom(src => src.Title));
    }
}