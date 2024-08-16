using PrintMatic.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class Review : BaseEntity
    {
        public string Message { get; set; }

        [Range(1 , 5)]
        public float? Rating { get; set; }

        public virtual Product Product { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public AppUser? AppUser { get; set; }
		[ForeignKey(nameof(AppUser))]
		public string? UserId { get; set; }

    }
}
