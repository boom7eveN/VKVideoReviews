using System.ComponentModel.DataAnnotations.Schema;

namespace VKVideoReviews.DA.Entities;

[Table("Reviews")]
public class ReviewEntity
{
    public Guid ReviewId { get; set; }
    public int Rate { get; set; }
    public string Text { get; set; }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; }

    public Guid VideoId { get; set; }
    public VideoEntity Video { get; set; }

    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}