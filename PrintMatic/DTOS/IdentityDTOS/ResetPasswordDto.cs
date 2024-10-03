using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
	public class ResetPasswordDto
	{
		[Required(ErrorMessage = "كلمه السر الجديده مطلوبه")]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{6,}$",
       ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، حرف صغير، رقم، رمز غير حرفي، وأن تكون على الأقل 6 أحرف.")]
        public string NewPassword { get; set; }

		[Required(ErrorMessage = "تأكيد كلمه السر مطلوب")]
		[Compare("NewPassword", ErrorMessage = " كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقتين.")]
        public string ConfirmPassword { get; set; }
    }
}
