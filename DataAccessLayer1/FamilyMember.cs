using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer1
{
    public class FamilyMember
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
