using System.ComponentModel.DataAnnotations.Schema;

namespace VKVideoReviews.DA.Entities;

[Table("VideoTypes")]
public class VideoTypeEntity
{
    public Guid VideoTypeId { get; set; }
    public string Title { get; set; }
    
    public ICollection<VideoEntity> Videos { get; set; } 
}