using System.ComponentModel.DataAnnotations;

namespace Thor.Models
{
    public class LoginModel
    {
        [Required]
        public string Password { get; set; }
    }
}
