namespace ZAnthe_BAckEnd.ViewModel.Pages
{
    public class HomeVM
    {
        public IEnumerable<Models.Slider> Sliders { get; set; }
        public Models.AboutBanner aboutBanners { get; set; }
        public IEnumerable<Models.Room> rooms { get; set; }
        public IEnumerable<Models.Service> services { get; set; }
        public IEnumerable<Models.Country> countries { get; set; }
        public Models.ContactUs Contact { get; set; }
        public Models.Country country { get; set; }
        public Models.Poster poster { get; set; }


    }
}
