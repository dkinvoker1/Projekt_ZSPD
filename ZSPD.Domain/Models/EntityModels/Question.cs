using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Domain.Models.EntityModels
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }

        public virtual Psychologist Author { get; set; }
        //[NotMapped]
        //public double AverageRate { get; set; }
    }
}
