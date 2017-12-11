
using System.Collections.Generic;
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Models
{
    public class EditSurveyViewModel
    {
        public Survey Survey { get; set; }
        public List<CreateSurveyViewModel> Questions { get; set; } 
    }
}