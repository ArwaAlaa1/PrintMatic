using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS

{
    public class LoginDto
    {
        [Required]
        public string EmailOrUserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
