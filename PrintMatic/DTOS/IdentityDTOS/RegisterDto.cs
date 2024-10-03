using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="اسم المستخدم مطلوب")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string Email { get; set; }

        [Required(ErrorMessage ="رقم الهاتف مطلوب")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "كلمه السر مطلوبه")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{6,}$",
        ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير، حرف صغير، رقم، رمز غير حرفي، وأن تكون على الأقل 6 أحرف.")]
        public string Password { get; set; }

		[Required(ErrorMessage = "تأكيد كلمه السر مطلوبه")]
		[Compare("Password", ErrorMessage = " كلمة المرور وتأكيد كلمة المرور غير متطابقتين.")]
        public string ConfirmPassword { get; set; }


    }
}
