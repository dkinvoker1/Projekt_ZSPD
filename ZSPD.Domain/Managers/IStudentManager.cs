﻿using System.Collections.Generic;

using ZSPD.Domain.Models.EntityModels.Accounts;
using ZSPD.Domain.Models.EntityModels;

namespace ZSPD.Domain.Managers
{
    public interface IStudentManager
    {
        bool SolvedAnyExercise(string userID);

        Models.EntityModels.Survey GetActiveSurvey(string userID);

        void SaveAnswers(List<Answer> answers, string userID);

        int GetStudentAdvacement(int advacement, string userId);

        int GetNextExcerciseNumber(int excerciseNumber, bool answer, string userID);

        int GetResolvedExerciseNumber(string userID);

        void SetActiveSubject(string userID, int subjectId);

        SubjectGraph GetActualSubject(string userId);
    }
}
