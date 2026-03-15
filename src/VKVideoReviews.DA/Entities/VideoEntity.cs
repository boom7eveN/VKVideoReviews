using System.ComponentModel.DataAnnotations.Schema;

namespace VKVideoReviews.DA.Entities;

[Table("Videos")]
public class VideoEntity
{
    public Guid VideoId { get; set; }
    public long VkVideoId { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public double AverageRate { get; set; }
    public int TotalReviews { get; set; }
    
    public Guid VideoTypeId { get; set; }
    public VideoTypeEntity VideoType { get; set; }
    
    public ICollection<GenresVideosEntity> GenresVideos { get; set; }
    public ICollection<ReviewEntity> Reviews { get; set; } 
    public ICollection<FavoriteEntity> Favorites {get; set; }
}