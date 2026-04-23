namespace VKVideoReviews.WebApi.Controllers.Requests.Pagination;

public class PageRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}