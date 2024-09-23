using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class Category: BaseEntity
    {
        [MaxLength(20)]
        public string Name { get; set; }

        public string PhotoURL { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
