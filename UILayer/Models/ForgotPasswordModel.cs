using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        public string Username { get; set; }

        [Required(ErrorMessage = "*Email address is required.")]
        [EmailAddress(ErrorMessage = "*Invalid email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "*Invalid email address format.")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "*password and confirm password mismatch.")]
        public string ConfirmPassword { get; set; }
    }
}