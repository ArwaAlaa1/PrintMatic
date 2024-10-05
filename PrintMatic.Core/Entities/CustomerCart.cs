using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class CustomerCart
    {
        public CustomerCart(string id)
        {
            Id = id;
            Items = new List<CartItems>();
        }

        public string Id { get; set; }
        public List<CartItems> Items { get; set; }
    }
}
