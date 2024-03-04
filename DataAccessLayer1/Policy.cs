using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public enum PolicyCategory
    {
      Life,
      Health,
      Motor,
      Travel
    }
    public class Policy
    {
        [Key]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "*Policy Number is required")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "*Applied Date is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfCreation { get; set; }

        public string Category { get; set; }


        public int CustomerId { get; set; }

        [Required(ErrorMessage = "*Policy Category is required")]
        public PolicyCategory policycategory {get; set; }

        public double Price { get; set; }
    }
    
}
