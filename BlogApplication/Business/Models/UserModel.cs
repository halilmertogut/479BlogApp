using DataAccess.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Business.Models
{
    public class UserModel : RecordBase
    {
        #region Entity Properties
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User Name is required!")]
        [StringLength(100, ErrorMessage = "User Name must be maximum {1} characters!")]
        public string Username { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(255, ErrorMessage = "Email must be maximum {1} characters!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(255, ErrorMessage = "Password must be maximum {1} characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } // Use Password for user input

        public string Id { get; set; } // Add Id property for user identifier
        #endregion
    }
}
