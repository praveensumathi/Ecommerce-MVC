using System.ComponentModel.DataAnnotations;

namespace EcommerceMVC.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
