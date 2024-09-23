using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
	public class ForgetPasswordDTO
	{
		
			[Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
			[EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
			public string Email { get; set; }
		

	}
}
