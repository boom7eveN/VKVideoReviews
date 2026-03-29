using VKVideoReviews.BL.Services.VideoTypes.Models;

namespace VKVideoReviews.WebApi.Controllers.Responses.VideoType;

public record VideoTypesListResponse(List<VideoTypeResponse> VideoTypes);