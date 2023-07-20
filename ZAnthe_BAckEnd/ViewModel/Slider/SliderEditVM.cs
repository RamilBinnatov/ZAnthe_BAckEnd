using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZAnthe_BAckEnd.ViewModel.Slider
{
    public class SliderEditVM
    {
        public string? Title { get; set; }
        public string? Header { get; set; }
        public string? Image { get; set; }
        public IFormFile Photo { get; set; }
    }
}
