using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities.Order
{
	public class Address
	{
        public Address()
        {
				
        }

		public Address(string fullName, string phoneNumber, string region, string city, string country, string? addressDetails)
		{
			FullName = fullName;
			PhoneNumber = phoneNumber;
			Region = region;
			City = city;
			Country = country;
			AddressDetails = addressDetails;
		}

		public string FullName { get; set; }
		public string PhoneNumber { get; set; }
		public string Region { get; set; }
		public string City { get; set; } 

		public string Country { get; set; }
		public string? AddressDetails { get; set; }
		
	}
}
