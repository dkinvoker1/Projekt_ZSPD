using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSPD.Domain.Models.EntityModels
{
    public class CompletedSurvey
    {
        public int Id { get; set; }

        public int SurveyId { get; set; }
        public virtual Survey Survey { get; set; }

        public DateTime DateOfComplete { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
