using PrintMatic.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class NotificationMessage: BaseEntity
    {
        public string Content { get; set; }
        public AppUser User { get; set; }
        [ForeignKey(nameof(AppUser))]
        public string UserId { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
