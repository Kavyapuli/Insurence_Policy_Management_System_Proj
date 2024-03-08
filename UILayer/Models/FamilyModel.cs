using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class FamilyModel
    {
        [Key]
        public int PersonId { get; set; }

        public string Name { get; set; }

        public string Relation { get; set; }
        //[ForeignKey("Id")]
        public int CustomerID { get; set; }

        public virtual Customer Customer { get; set; }

    }
}