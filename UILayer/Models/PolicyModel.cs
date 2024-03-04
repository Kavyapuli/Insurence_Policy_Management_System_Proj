using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataAccessLayer;
namespace UILayer.Models
{
    public class PolicyModel
    {
        [Required(ErrorMessage = "*Policy Number is required")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "*Applied Date is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfCreation { get; set; }

        public string Category { get; set; }
        public PolicyCategory policycategory { get; set; }

        public double Price { get; set; }

    }
}