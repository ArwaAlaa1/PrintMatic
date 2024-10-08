using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
	public class VerifyCodeDto
	{
		[Required(ErrorMessage ="البريد الالكترونى مطلوب")]
		public string Email { get; set; }
		[Required(ErrorMessage = "تأكيد الكود مطلوب")]
		public string Code { get; set; }
    }
}
