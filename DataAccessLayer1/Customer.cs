using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "*First name is required.")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*Last name is required.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*Email address is required.")]
        [EmailAddress(ErrorMessage = "*Invalid email address.")]
        [Index("IX_UniqueEmpUserEmail", IsUnique = true)]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Phone number is required.")]
        [Phone(ErrorMessage = "*Invalid phone number.")]
        [RegularExpression(@"^(?:[6-9][0-9]{9})$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "*Username is required.")]
        [MaxLength(255)]
        [Index("IX_UniqueEmpUserName", IsUnique = true)]

        public string UserName { get; set; }

        [Required(ErrorMessage = "*Password is required.")]
        [DataType(DataType.Password)]
        [MaxLength(255)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must have at least 8 characters, one uppercase letter, one lowercase letter, and one digit.")]
        public string Password { get; set; }

        public int RoleId { get; set; }
        public virtual Roles Role { get; set; }
    }
}
