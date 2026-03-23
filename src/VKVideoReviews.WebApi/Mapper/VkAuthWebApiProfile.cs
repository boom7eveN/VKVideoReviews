using AutoMapper;
using VKVideoReviews.BL.Services.VkAuth.Models;
using VKVideoReviews.WebApi.Controllers.Requests.VkAuth;

namespace VKVideoReviews.WebApi.Mapper;

public class VkAuthWebApiProfile : Profile
{
    public VkAuthWebApiProfile()
    {
        CreateMap<VkAuthCallbackRequest, VkAuthCallbackModel>();
    }
}