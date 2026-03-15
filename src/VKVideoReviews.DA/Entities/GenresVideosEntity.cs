using System.ComponentModel.DataAnnotations.Schema;

namespace VKVideoReviews.DA.Entities;

[Table("GenresVideos")]
public class GenresVideosEntity
{
    public Guid GenreId { get; set; }
    public GenreEntity Genre { get; set; }
    
    public Guid VideoId { get; set; }
    public VideoEntity Video { get; set; }
}