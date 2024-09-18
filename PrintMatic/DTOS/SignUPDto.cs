using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS
{
	public class SignUPDto
	{
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*\\W).{6,}$",
            ErrorMessage = "write Password contain at least 1 uppercase character ,1 lowercase ,1 number ,1 non alphabetic and at least 6 charachter")]
        public string Password { get; set; }


    }
}
