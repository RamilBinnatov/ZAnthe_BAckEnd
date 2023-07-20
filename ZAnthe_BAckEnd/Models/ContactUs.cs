using System.ComponentModel.DataAnnotations;

namespace ZAnthe_BAckEnd.Models
{
    public class ContactUs : BaseEntity
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
    }
}
