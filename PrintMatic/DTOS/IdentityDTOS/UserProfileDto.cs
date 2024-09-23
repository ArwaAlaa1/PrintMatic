using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
    public class UserProfileDto
    {
        [Required(ErrorMessage ="اسم المستخدم مطلوب")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "البريد الاكترونى مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string Email { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string PhoneNumber { get; set; }
    }
}
