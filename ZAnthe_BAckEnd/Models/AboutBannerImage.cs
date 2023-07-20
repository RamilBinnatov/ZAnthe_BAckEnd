namespace ZAnthe_BAckEnd.Models
{
    public class AboutBannerImage : BaseEntity
    {
        public string? Image { get; set; }
        public int AboutBannerImageId { get; set; }
        public AboutBanner AboutBanner { get; set; }
    }
}
