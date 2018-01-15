using System.Collections.Generic;

namespace ZSPD.Domain.Models.EntityModels.Accounts
{
    public class Student : AppUser
    {
        public virtual Survey ActiveSurvey { get; set; }
        public bool SurveyIsStarted { get; set; }

        public virtual ICollection<CompletedSurvey> CompletedSurveys { get; set; }

        public virtual ICollection<Excercise> ProperlySolvedExcercises { get; set; }

        public virtual SubjectGraph ActualSubject { get; set; }
    }
}
