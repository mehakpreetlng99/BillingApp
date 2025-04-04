using System.ComponentModel.DataAnnotations;

namespace BillingApp.DTO
{
    public class UserDTO
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long, 1 special character and should contain alpha-numeric")]
        public string? Password { get; set; } 

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = string.Empty;

        public string? AdminId { get; set; }
    }
}
