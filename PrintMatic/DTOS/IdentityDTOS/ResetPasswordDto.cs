using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
	public class ResetPasswordDto
	{
		[Required(ErrorMessage = "البريد الالكترونى مطلوب")]
		public string Email { get; set; }
		[Required(ErrorMessage = "كلمه السر الجديده مطلوبه")]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{5,}$",
    ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، حرف صغير، رقم، وأن تكون على الأقل 5 أحرف.")]

		public string NewPassword { get; set; }

		[Required(ErrorMessage = "تأكيد كلمه السر مطلوب")]
		[Compare("NewPassword", ErrorMessage = " كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقتين.")]
        public string ConfirmPassword { get; set; }
    }
}
