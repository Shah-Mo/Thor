using System.ComponentModel.DataAnnotations;

namespace Thor.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Password { get; set; }
    }
}
