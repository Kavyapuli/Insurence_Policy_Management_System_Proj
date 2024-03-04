using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "*UserName is required")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*Password is required.")]
        [DataType(DataType.Password)]
        [MaxLength(255)]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "*password and confirm password mismatch.")]
        [DataType(DataType.Password)]
        [MaxLength(255)]
        [Display(Name = "*Confirm Password")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}