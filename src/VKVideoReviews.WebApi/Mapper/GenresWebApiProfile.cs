using AutoMapper;
using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.WebApi.Controllers.Requests.Genre;

namespace VKVideoReviews.WebApi.Mapper;

public class GenresWebApiProfile : Profile
{
    public GenresWebApiProfile()
    {
        CreateMap<CreateGenreRequest, CreateGenreModel>();
        CreateMap<UpdateGenreRequest, UpdateGenreModel>();
    }
}