using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string PasswordHash { get; set; }
        #endregion

    }
}
