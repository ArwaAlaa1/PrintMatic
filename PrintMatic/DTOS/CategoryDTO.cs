using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }

        public string PhotoURL { get; set; }
        public string Description { get; set; }
    }
}
