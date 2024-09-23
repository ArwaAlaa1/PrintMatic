using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
	public class ResetPasswordDto
	{

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{6,}$",
       ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، حرف صغير، رقم، رمز غير حرفي، وأن تكون على الأقل 6 أحرف.")]
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = " كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقتين.")]
        public string ConfirmPassword { get; set; }
    }
}
