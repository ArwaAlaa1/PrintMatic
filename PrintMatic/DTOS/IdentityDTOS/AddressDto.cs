using PrintMatic.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.IdentityDTOS
{
    public class AddressDto
    {

       
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string? AddressDetails { get; set; }
       
    }
}
