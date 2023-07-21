using System.ComponentModel.DataAnnotations.Schema;

namespace ZAnthe_BAckEnd.Models
{
    public class RoomService : BaseEntity
    {
        public string? Img { get; set; }
        public string? Name { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
        public ICollection<RoomServicePivot> RoomServicePivots { get; set; }

    }
}
