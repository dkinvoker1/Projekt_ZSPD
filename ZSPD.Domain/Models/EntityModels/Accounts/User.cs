using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Domain.Models.EntityModels.Accounts
{
    public class User : AppUser
    {
        public virtual Survey ActiveSurvey { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public bool SurveyIsStarted { get; set; }

        // ICollection<Survey> Surveys 
    }
}
