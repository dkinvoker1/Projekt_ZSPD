using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Models
{
    public class SurveyToAssign
    {
        public Survey Survey { get; set; }
        public bool ToAssign { get; set; }
    }
}