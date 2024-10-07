using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS
{
    public class FavouriteDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="كود المنتج مطلوب")]
       public int ProductId { get; set; }
        [Required(ErrorMessage = "كود المستخدم مطلوب")]
        public string UserId { get; set; }
    }
}
