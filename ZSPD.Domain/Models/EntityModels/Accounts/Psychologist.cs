using System.Collections.Generic;

namespace ZSPD.Domain.Models.EntityModels.Accounts
{
    public class Psychologist : AppUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }
        public virtual ICollection<Grade> QuestionsGrades { get; set; }
    }
}
