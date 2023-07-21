namespace ZAnthe_BAckEnd.Models
{
    public class Room : BaseEntity
    {
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Desc { get; set; }
        public ICollection<RoomServicePivot> RoomServicePivots { get; set; }
        public int Price { get; set; }
    }
}
