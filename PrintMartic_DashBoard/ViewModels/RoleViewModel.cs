using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.ViewModels
{
	public class RoleViewModel
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string RoleName { get; set; }
    }
}
