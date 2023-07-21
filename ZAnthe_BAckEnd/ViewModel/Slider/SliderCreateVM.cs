using System.ComponentModel.DataAnnotations;

namespace ZAnthe_BAckEnd.ViewModel.Slider
{
    public class SliderCreateVM
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Header { get; set; }

        public IFormFile? Photo { get; set; }
    }
}
