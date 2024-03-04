using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Category
    {
       
        
            [Key]
            public int CategoryId { get; set; }

            [Required(ErrorMessage = "Policy Type is required")]
            [MaxLength(50)]
            public string CategoryName { get; set; }
            public virtual ICollection<Policy> Policies { get; set; }
        
    }
}
