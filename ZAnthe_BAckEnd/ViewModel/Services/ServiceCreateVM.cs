using System.ComponentModel.DataAnnotations;

namespace ZAnthe_BAckEnd.ViewModel.Services
{
    public class ServiceCreateVM
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Desc { get; set; }

        public IFormFile? Photo { get; set; }
    }
}
