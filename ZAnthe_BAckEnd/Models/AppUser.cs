using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg;

namespace ZAnthe_BAckEnd.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool isActive { get; set; } = false;
    }
}
