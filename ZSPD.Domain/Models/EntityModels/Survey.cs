using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Domain.Models.EntityModels
{
    public class Survey
    {
        public int Id { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        public virtual Psychologist Author { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
