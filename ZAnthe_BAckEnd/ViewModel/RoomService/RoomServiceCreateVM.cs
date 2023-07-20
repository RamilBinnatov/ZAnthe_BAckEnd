using System.ComponentModel.DataAnnotations;

namespace ZAnthe_BAckEnd.ViewModel.RoomService
{
    public class RoomServiceCreateVM
    {
        [Required]
        public string Img { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
