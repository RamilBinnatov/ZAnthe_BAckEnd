using System.ComponentModel.DataAnnotations;

namespace ZAnthe_BAckEnd.ViewModel.Account
{
    public class ForgetPassword
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
