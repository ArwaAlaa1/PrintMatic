using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.ViewModels
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage ="Name Role Is Required")]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
