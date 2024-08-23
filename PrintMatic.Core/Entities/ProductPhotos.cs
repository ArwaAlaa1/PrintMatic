using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class ProductPhotos : Entity
    {
        public string Photo {  get; set; }

        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

        public string FilePath { get; set; }

       public bool Enter { get; set; }
    }
}
