using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZAnthe_BAckEnd.Models;

namespace ZAnthe_BAckEnd.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<AboutBanner> aboutBanners { get; set; }
        public DbSet<Awards> awards { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<ContactUs> contactUs { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<HotelFeatures> hotelFeatures { get; set; }
        public DbSet<Poster> posters { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<RoomService> roomServices { get; set; }
        public DbSet<RoomServicePivot> roomServicePivots { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<Slider> sliders { get; set; }
        public DbSet<Social> socials { get; set; }
    }
}
