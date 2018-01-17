using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSPD.Domain.Models.EntityModels;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Models
{
    public class StudentDetailsViewModel
    {
        public Student ActualStudent { get; set; }
        public List<SubjectGraph> AllSubjects { get; set; }

        public string SelectedSubject { get; set; }

        public StudentDetailsViewModel()
        {

        }

        public StudentDetailsViewModel(Student student, List<SubjectGraph> subjectGraphs)
        {
            ActualStudent = student;
            AllSubjects = subjectGraphs.ToList();
            if (student.ActualSubject == null)
            {
                SelectedSubject = AllSubjects.First().Name;
            }
            else
            {
                SelectedSubject = student.ActualSubject.Name;
            }
        }
    }
}