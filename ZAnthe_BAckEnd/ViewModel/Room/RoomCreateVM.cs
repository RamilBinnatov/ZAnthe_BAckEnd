using Microsoft.Build.Framework;
using ZAnthe_BAckEnd.Models;

namespace ZAnthe_BAckEnd.ViewModel.Room
{
    public class RoomCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Desc { get; set; }
        public int Price { get; set; }
        public int RoomServiceId { get; set; }
        public List<Models.RoomService> RoomServices { get; set; }
        public IEnumerable<int> RoomServicePivotList { get; set; }
    }
}
