using System.ComponentModel.DataAnnotations.Schema;

namespace VKVideoReviews.DA.Entities;

[Table("Genre")]
public class GenreEntity
{
    public Guid GenreId { get; set; }
    public string Title { get; set; }
    
    public ICollection<GenresVideosEntity> GenresVideos { get; set; }
}