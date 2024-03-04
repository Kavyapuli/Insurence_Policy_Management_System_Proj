using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DataAccessLayer
{
    public class Admin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*First name is required.")]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="*Last name is required.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage ="*Email address is required.")]
        [EmailAddress(ErrorMessage ="*Invalid email address.")]
        [MaxLength(100)]
        [Index("IX_UniqueAdminEmail", IsUnique = true)]
        public string EmailAddress {  get; set; }
        [Required(ErrorMessage = "*Phone number is required")]
        [Phone(ErrorMessage ="*Invalid PhoneNumber is required")]
        [RegularExpression(@"^(?:[6-9][0-9]{9})$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber {  get; set; }
        [Required(ErrorMessage = "*UserName is required.")]
        [MaxLength(255)]
        [Index("IX_UniqueAdminUserName", IsUnique = true)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*Password is required .")]
        [DataType(DataType.Password)]
        [MaxLength(100)]
        public string Password { get; set; }
        public int RoleId { get; set; }
        public virtual Roles Role { get; set; }


    }
}
