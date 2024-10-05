using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS

{
    public class LoginDto
    {
        [Required(ErrorMessage ="اسم المستخدم او البريد الالكترونى مطلوب")]
        public string EmailOrUserName { get; set; }


        [Required(ErrorMessage ="كلمة السر مطلوبه")]
        public string Password { get; set; }
    }
}
