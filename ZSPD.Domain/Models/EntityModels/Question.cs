using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZSPD.Domain.Models.EntityModels
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }

        //[NotMapped]
        //public double AverageRate { get; set; }
    }
}
