using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Models
{
    public class StudentToAssign
    {
        public Student Student { get; set; }
        public bool ToAssign { get; set; }
    }
}