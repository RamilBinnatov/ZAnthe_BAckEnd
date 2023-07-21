using System.ComponentModel.DataAnnotations;

namespace ZAnthe_BAckEnd.ViewModel.Room
{
    public class RoomEditVM
    {
        public string? Image { get; set; }
        public IFormFile Photo { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Desc { get; set; }
        [Required]
        public int GuestCount { get; set; }
        [Required]
        public int MinBookingLim { get; set; }
        [Required]
        public string BedType { get; set; }
        [Required]
        public int Area { get; set; }
        public int Price { get; set; }
   
    }
}
