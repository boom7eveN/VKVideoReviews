using VKVideoReviews.WebApi.Controllers.Requests.Pagination;

namespace VKVideoReviews.WebApi.Controllers.Requests.Videos;

public class VideosPageRequest : PageRequest
{
    public string? TitlePart { get; set; }
}