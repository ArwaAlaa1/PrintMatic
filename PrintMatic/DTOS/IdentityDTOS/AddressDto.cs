using PrintMatic.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
    public class AddressDto
    {
		//public int? Id { get; set; }

		[Required(ErrorMessage ="هذا الحقل مطلوب")]
        public string FullName { get; set; }
		[Required(ErrorMessage = "هذا الحقل مطلوب")]
		public string PhoneNumber { get; set; }
		[Required(ErrorMessage = "هذا الحقل مطلوب")]
		public string Region { get; set; }
		
        public int? CityId { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string City { get; set; }
		[Required(ErrorMessage = "هذا الحقل مطلوب")]
		public string Country { get; set; }
        public string? AddressDetails { get; set; }
		//public string? AppUserId { get; set; }
	}
}
