using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using ZSPD.Domain.Models.EntityModels;


namespace ZSPD.Models
{
    public class SurveyViewModel
    {
        public Survey activeSurvey { get; set;}
        public List<Answer> Answers { get; set; }
    }
}