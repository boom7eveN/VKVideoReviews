using AutoMapper;
using VKVideoReviews.BL.Common.Pagination;
using VKVideoReviews.BL.Common.Paging;
using VKVideoReviews.WebApi.Controllers.Requests.Pagination;
using VKVideoReviews.WebApi.Controllers.Requests.Videos;
using VKVideoReviews.WebApi.Controllers.Responses.Pagination;

namespace VKVideoReviews.WebApi.Mapper;

public class PaginationWebApiProfile : Profile
{
    public PaginationWebApiProfile()
    {
        CreateMap<PageRequest, PageRequestModel>();
        CreateMap<VideosPageRequest, VideosFilterModel>();

        CreateMap(typeof(PagedListModel<>), typeof(PagedResponse<>))
            .ForMember(nameof(PagedResponse<object>.TotalPages),
                opt => opt.MapFrom(nameof(PagedListModel<object>.TotalPages)));
    }
}