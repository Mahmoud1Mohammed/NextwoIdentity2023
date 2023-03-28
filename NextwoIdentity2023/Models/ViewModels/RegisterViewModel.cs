using System.ComponentModel.DataAnnotations;

namespace NextwoIdentity2023.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter Email")]
        [EmailAddress]
        public String? Email { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public String? Password { get; set; }
        [Required(ErrorMessage = "Enter Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Confirm Amd Password Not Match")]

        public String? ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Enter Phone")]

        public String? Phone { get; set; }



    }
}
