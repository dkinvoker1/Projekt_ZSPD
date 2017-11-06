using System.Collections.Generic;

namespace ZSPD.Domain.Models.EntityModels.Accounts
{
    public class Student : AppUser
    {
        public virtual Survey ActiveSurvey { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public bool SurveyIsStarted { get; set; }

        // ICollection<Survey> Surveys 
    }
}
