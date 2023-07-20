using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace ZAnthe_BAckEnd.Models
{
    public class RoomServicePivot 
    {
        public int Id { get; set; }

        public int RoomServiceId { get; set; }
        public int RoomId { get; set; }

        public RoomService roomService { get; set; }
        public Room room { get; set; }

    }
}
