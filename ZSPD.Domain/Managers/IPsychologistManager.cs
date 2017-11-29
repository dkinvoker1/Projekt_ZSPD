using System.Collections.Generic;

using ZSPD.Domain.Models.EntityModels;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Domain.Managers
{
    public interface IPsychologistManager
    {
        // Get
        List<Student> GetAllStudents();        

        List<Question> GetAllQuestions();

        List<Survey> GetAllSurveys();
        List<Survey> GetOwnSurveys(string userId);
        List<Survey> GetSurvey(string surveyId);

        Psychologist GetPsychologist(string userId);

        // Update
        void RateQuestions(List<Grade> grades);

        // Add
        void AddSurvey(Survey survey);
        void AddQuestion(Question question);

        void AddSurveyToStudent(Survey survey, string studentId);

		//...

		void AddOrUpdateRateQuestion(int? grade, string userId, int questionId);
    }
}
