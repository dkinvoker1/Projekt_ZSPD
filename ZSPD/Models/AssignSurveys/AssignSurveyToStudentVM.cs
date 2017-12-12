using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Models
{
    public class AssignSurveyToStudentVM
    {
        public List<Survey> SurveysToAssign { get; set; }
        public List<StudentToAssign> StudentsToAssign { get; set; }
        public int SelectedSurveyId { get; set; }

    }
}