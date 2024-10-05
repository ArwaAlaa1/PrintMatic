using Microsoft.AspNetCore.Identity;
using PrintMatic.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Services
{
	public  class UserService : IUserService
	{
		public  string GetCustomErrorsMessage(IdentityError error)
		{
			return error.Code switch
			{
				"PasswordRequiresNonAlphanumeric" => "يجب أن تحتوي كلمة المرور على حرف خاص واحد على الأقل",
				"PasswordRequiresDigit" => "يجب أن تحتوي كلمة المرور على رقم واحد على الأقل",
				"PasswordRequiresUpper" => "يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل",
				"PasswordTooShort"  => " كلمة المرور قصيرة جدًا؛ يجب أن تكون 6 أحرف على الأقل ",
				"DuplicateUserName" => "اسم المستخدم مستخدم بالفعل. يرجى اختيار اسم آخر",
				"DuplicateEmail" => "عنوان البريد الإلكتروني مستخدم بالفعل. يرجى استخدام بريد إلكتروني مختلف.",
				_ => error.Description
			};
		}
	}
}
