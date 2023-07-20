namespace ZAnthe_BAckEnd.ViewModel.Awards
{
    public class AwardsListVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
    }
}
