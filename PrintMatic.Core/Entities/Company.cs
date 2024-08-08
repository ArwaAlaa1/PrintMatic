using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class Company : BaseEntity
	{
		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		[Required]
		[Phone]
		public string Phone { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Url]
		public string Logo { get; set; }

		
		[StringLength(50)]
		public string BankAccount { get; set; }

		[StringLength(200)]
		public string Location { get; set; }

		[StringLength(50)]
		public string WayOfReceiveMoney { get; set; }
	}


}
