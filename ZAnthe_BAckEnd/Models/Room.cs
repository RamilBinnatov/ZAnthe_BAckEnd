namespace ZAnthe_BAckEnd.Models
{
    public class Room : BaseEntity
    {
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public int GuestCount { get; set; }
        public int MinBookingLim { get; set; }
        public string BedType { get; set; }
        public int Area { get; set; }
        public ICollection<RoomServicePivot> RoomServicesPivot { get; set; }
        public int Price { get; set; }
    }
}
