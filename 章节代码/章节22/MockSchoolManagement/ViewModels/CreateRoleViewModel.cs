using System.ComponentModel.DataAnnotations;

namespace MockSchoolManagement.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "角色")]
        public string RoleName { get; set; }
    }
}
