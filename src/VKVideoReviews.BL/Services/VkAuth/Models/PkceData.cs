namespace VKVideoReviews.BL.Services.VkAuth.Models;

public class PkceData
{
    public string CodeVerifier { get; set; }
    public string CodeChallenge { get; set; }
}