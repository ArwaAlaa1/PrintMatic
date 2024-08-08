using PrintMatic.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
    public class ReviewVM
    {
        public int Id { get; set; }

        public string Message { get; set; }

        [Range(1, 5)]
        public float? Rating { get; set; }

        public virtual Product? Product { get; set; }
        
        public int ProductId { get; set; }
    }
}
