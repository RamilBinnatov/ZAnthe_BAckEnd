namespace ZAnthe_BAckEnd.ViewModel.Pages
{
    public class RoomDetailVM
    {
        public Models.Room room { get; set; }
        public IEnumerable<Models.Room> SimilarRooms { get; set; }
        public List<Models.RoomService> roomServices { get; set; }

    }
}
