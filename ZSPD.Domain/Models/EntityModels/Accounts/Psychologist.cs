using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

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
