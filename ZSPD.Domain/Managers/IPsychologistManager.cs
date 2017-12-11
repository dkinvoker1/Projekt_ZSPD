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
        Survey GetSurvey(int surveyId);

        Psychologist GetPsychologist(string userId);

        // Update
        void AddOrUpdateRateQuestion(int? grade, string userId, int questionId);
        void EditSurvey(List<Question> questions, int surveyId);

        // Add
        void AddSurvey(List<Question> questions, string authorId);
        void AddQuestion(string question);

        void AddSurveyToStudent(Survey survey, string studentId);

        // Remove
        void RemoveSurvey(int surveyId);

		//...

    }
}
