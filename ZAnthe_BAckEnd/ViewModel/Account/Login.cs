using System.ComponentModel.DataAnnotations;

namespace ZAnthe_BAckEnd.ViewModel.Account
{
    public class Login
    {
        [Required]
        public string? UserNameorEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
