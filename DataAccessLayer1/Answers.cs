using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Answers
    {
        [Key]
        [Required(ErrorMessage = "*Message Box is Empty")]
        public int AnswerId { get; set; }

        [ForeignKey("Question")]
        public int QuestionSLNO { get; set; }

        public virtual Questions Question { get; set; }

        [MaxLength(255)]
        public string AnswerText { get; set; }

        [DataType(DataType.Date)]
        public DateTime AnswerDate { get; set; } = DateTime.Now;
    }
}
