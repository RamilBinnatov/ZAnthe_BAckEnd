using System.ComponentModel.DataAnnotations;

namespace ZAnthe_BAckEnd.ViewModel.Awards
{
    public class AwardsCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
