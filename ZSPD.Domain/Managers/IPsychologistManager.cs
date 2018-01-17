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
        List<Question> GetOwnQuestions(string userId);

        List<Survey> GetAllSurveys();
        List<Survey> GetOwnSurveys(string userId);
        Survey GetSurvey(int surveyId);

        Psychologist GetPsychologist(string userId);

        // Update
        void AddOrUpdateRateQuestion(int? grade, string userId, int questionId);
        void EditSurvey(List<Question> questions, int surveyId);
        void EditQuestion(Question question);

        // Add
        void AddSurvey(List<Question> questions, string authorId);
        void AddQuestion(string question, string authorId);
        void AddQuestionFromExcel(string path, string authorId);

        void AddSurveyToStudent(int surveyId, string studentId);

        // Remove
        void RemoveSurvey(int surveyId);
        void RemoveQuestion(Question question);

		//...

    }
}
