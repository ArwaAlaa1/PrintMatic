using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
	public class EmailDto
	{

		[Required(ErrorMessage = "البريد الالكترونى مطلوب")]
		public string Email { get; set; }
	}
}
