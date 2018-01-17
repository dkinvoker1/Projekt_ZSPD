using System;
using System.Collections.Generic;
using ZSPD.Domain.Models.EntityModels.Accounts;
using ZSPD.Domain.Models.EntityModels;
using System.Linq;
using System.IO;

using ZSPD.Domain.Models;
using System.Reflection;
using System.Web;

namespace ZSPD.Domain.Managers
{
    public class StudentManager : IStudentManager
    {
        public StudentManager()
        {
        }

        public Models.EntityModels.Survey GetActiveSurvey(string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var survey = db.Students.FirstOrDefault(x => x.Id == userID).ActiveSurvey;
                if (survey != null)
                {
                    survey.Questions = survey.Questions.ToList();
                    return survey;
                }
                return null;
            }
        }

        public bool SolvedAnyExercise(string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Students.FirstOrDefault(x => x.Id == userID);
                if(user != null && user.ProperlySolvedExcercises.Where(ex => ex.graph.Id == user.ActualSubject.Id).Count() > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public void SetActiveSubject(string userID, int subjectId)
        {
            using (var context = new ApplicationDbContext())
            {
                var subject = context.SubjectGraphs.First(x => x.Id == subjectId);
                var user = context.Students.FirstOrDefault(x => x.Id == userID);
                user.ActualSubject = subject;

                context.SaveChanges();
            }
        }

        public int GetResolvedExerciseNumber(string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Students.FirstOrDefault(x => x.Id == userID);
                if (user != null && user.ProperlySolvedExcercises.Where(ex => ex.graph.Id == user.ActualSubject.Id).Count() > 0)
                {
                    return user.ProperlySolvedExcercises.Where(ex => ex.graph.Id == user.ActualSubject.Id).Last().excerciseNumber;
                }
                return 0;
            }
        }

        public void SaveAnswers(List<Answer> answers, string userID)
        {
            var survey = GetActiveSurvey(userID);
            var questions = survey.Questions.ToList();

            for(int i = 0; i < questions.Count; i++)
            {
                answers[i].QuestionId = questions[i].Id;
            }

            var completedSurvey = new CompletedSurvey()
            {
                Answers = answers,
                DateOfComplete = DateTime.Now,
                SurveyId = survey.Id
            };

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Students.FirstOrDefault(x => x.Id == userID);

                if (user != null)
                {
                    user.CompletedSurveys.Add(completedSurvey);

                    user.SurveyIsStarted = true;
                    db.SaveChanges();
                }
            }
        }

        public int GetExcerciseIssueNumber(int excerciseNumber, string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Students.FirstOrDefault(x => x.Id == userId);

                int[,] pojecian;
                int[,] pojeciap;
                int[,] zadania;

                GetExcerciseOrder(out pojecian, out pojeciap, out zadania, userId);

                int pojecie = 0;
                for (int i = 0; i < zadania.GetLength(0); i++)
                {
                    for (int j = 0; j < zadania.GetLength(1); j++)
                    {
                        if (zadania[i, j] == excerciseNumber)
                        {
                            pojecie = i;
                        }
                    }
                }

                return pojecie;
            }
        }

        public bool ExcerciseSolutionStatus(int excerciseNumber, string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Students.FirstOrDefault(x => x.Id == userID);
                var solvedExcercises = user.ProperlySolvedExcercises.Where(x => x.graph.Id == user.ActualSubject.Id);
                if (solvedExcercises.Where(x => x.excerciseNumber == excerciseNumber).Any())
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool IssueSolutionStatus(int issueNumber, string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Students.FirstOrDefault(x => x.Id == userID);
                var solvedIssues = user.SolvedIssues.Where(x => x.graph.Id == user.ActualSubject.Id);
                if (solvedIssues.Where(x => x.issueNumber == issueNumber).Any())
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public void SaveSolvedExcercise(int excerciseNumber, string userID)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Students.FirstOrDefault(x => x.Id == userID);
                int issueNumber = GetExcerciseIssueNumber(excerciseNumber, userID);
                user.ProperlySolvedExcercises.Add(new Excercise(excerciseNumber, user.ActualSubject));
                user.SolvedIssues.Add(new Issue(issueNumber, user.ActualSubject));
                db.SaveChanges();
            }
        }

        public void GetExcerciseOrder(out int[,] pojecian, out int[,] pojeciap, out int[,] zadania, string userId)
        {
            Student user;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                user = db.Students.FirstOrDefault(x => x.Id == userId);
                if (user.ActualSubject == null)
                {
                    user.ActualSubject = db.SubjectGraphs.First();
                    db.SaveChanges();
                }
            }

            // wczytanie z pliku tekstowego wartosci dla nastepnikow
            string poj = user.ActualSubject.NextConcepts;

            //System.IO.File.ReadAllText(String.Format(@"{0}\ZSPD.Domain\Resources\Lista_pojec.txt", mainPath));
            // wczytanie z pliku tekstowego wartosci dla poprzednikow
            string poj1 = user.ActualSubject.PreviousConcepts;
            //System.IO.File.ReadAllText(String.Format(@"{0}\ZSPD.Domain\Resources\Lista_pojec1.txt", mainPath));
            // wczytanie z pliku tekstowego zadan dla odp pojec
            string zad = user.ActualSubject.Exercises;
                //System.IO.File.ReadAllText(String.Format(@"{0}\ZSPD.Domain\Resources\Lista_zadan.txt", mainPath));

            string[] roz = poj.Split(';');
            string[] split = poj1.Split(';');
            string[] dew = zad.Split(';');

            // wczytywanie listy nastepnikow do tablicy
            // elastyczne rozwiązanie - nie delkarujemy na sztywno wymiarów. Nie zabezpieczone przed błędami
            int ileZadan = 0;
            for (int i = 0; i < roz.Length; i++)
            {
                int ile = roz[i].Split(',').Length;
                if(ile> ileZadan)
                {
                    ileZadan = ile;
                }
            }

            pojecian = new int[roz.Length, ileZadan]; 
            for (int i = 0; i < roz.Length; i++)
            {
                string[] roz1 = roz[i].Split(',');
                for (int j = 0; j < roz1.Length; j++)
                {
                    pojecian[i, j] = int.Parse(roz1[j]);
                }
            }

            // wczytywanie listy poprzednikow do tablicy
            ileZadan = 0;
            for (int i = 0; i < split.Length; i++)
            {
                int ile = split[i].Split(',').Length;
                if (ile > ileZadan)
                {
                    ileZadan = ile;
                }
            }

            pojeciap = new int[split.Length, ileZadan];
            for (int i = 0; i < split.Length; i++)
            {
                string[] split1 = split[i].Split(',');
                for (int j = 0; j < split1.Length; j++)
                {

                    pojeciap[i, j] = int.Parse(split1[j]);
                }
            }

            //wczytywanie listy zadan do tablicy
            ileZadan = 0;
            for (int i = 0; i < dew.Length; i++)
            {
                int ile = dew[i].Split(',').Length;
                if (ile > ileZadan)
                {
                    ileZadan = ile;
                }
            }

            zadania = new int[dew.Length, ileZadan];
            for (int i = 0; i < dew.Length; i++)
            {
                string[] dew1 = dew[i].Split(',');
                for (int j = 0; j < dew1.Length; j++)
                {

                    zadania[i, j] = int.Parse(dew1[j]);
                }
            }
        }

        public int GetStudentAdvacement(int advacement, string userId)
        {
            int[,] pojecian;
            int[,] pojeciap;
            int[,] zadania;

            GetExcerciseOrder(out pojecian, out pojeciap, out zadania, userId);


            Random sectionRand = new Random();
            Random excerciseRand = new Random();

            int sectionNumber;
            int excercisePosition;

            int ilosc = 0;
            int iloscz = 0;

            if (advacement == 1)
            {
                sectionNumber = sectionRand.Next(0, 4);
            }
            else if(advacement == 2)
            {
                sectionNumber = sectionRand.Next(5, 9);
            }
            else if (advacement == 3)
            {
                sectionNumber = sectionRand.Next(10, 14);
            }
            else if (advacement == 4)
            {
                sectionNumber = sectionRand.Next(15, 19);
            }
            else
            {
                sectionNumber = sectionRand.Next(20, 22);
            }

            while (iloscz < 4 && zadania[sectionNumber, ilosc] != 0)
            {
                iloscz++;

                if (ilosc < 2)
                {
                    ilosc++;
                }
            }

            excercisePosition = excerciseRand.Next(0, iloscz - 1);

            
            // dodawanie do wektora wiedzy z pojęciami poprzednich pojęć
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Students.FirstOrDefault(x => x.Id == userId);
                int issueNumber = GetExcerciseIssueNumber(zadania[sectionNumber, excercisePosition], userId);
                for (int i = 0; i < issueNumber; i++)
                {
                    user.SolvedIssues.Add(new Issue(i, user.ActualSubject));
                }
                db.SaveChanges();
            }

            return zadania[sectionNumber, excercisePosition];

        }

        public int GetIssuesCount(int[,] pojecia, int start)
        {
            int size = pojecia.GetLength(1);
            List<int> nastepnePojecia = new List<int>();
            for(int i = 0; i < size; i++)
            {
                nastepnePojecia.Add(pojecia[start, i]);
            }
            int iloscPojec = nastepnePojecia.Where(n => n != 0).Count();
            return iloscPojec;
        }

        public int GetExcerciseNumberFromIssue(int iloscz, int start, int[,] pojecia, string userID)
        {
            int[,] pojecian;
            int[,] pojeciap;
            int[,] zadania;

            GetExcerciseOrder(out pojecian, out pojeciap, out zadania, userID);

            Random x = new Random();
            int wylosowana = x.Next(1, iloscz + 1);
            int wylosowanePojecie = pojecia[start, wylosowana - 1];
            if (wylosowanePojecie == 0)
            {
                return GetExcerciseNumberFromIssue(1, start + 1, pojecia, userID);
            }
            int iloscZadan = GetIssuesCount(zadania, wylosowanePojecie - 1);
            int indexZadania = x.Next(0, iloscZadan);
            int zadanie = zadania[wylosowanePojecie - 1, indexZadania];
            if (!ExcerciseSolutionStatus(zadanie, userID) && !IssueSolutionStatus(GetExcerciseIssueNumber(zadanie, userID), userID))
            {
                return zadanie;
            }
            else
            {
                if (start != zadania.GetLength(0) - 1)
                {
                    return GetExcerciseNumberFromIssue(GetIssuesCount(pojecia, start + 1), start + 1, pojecia, userID);
                }
                else
                {
                    var lastExcercises = new List<int>();

                    for (int i = 0; i < GetIssuesCount(zadania, start); i++)
                    {
                        lastExcercises.Add(zadania[start, i]);
                    }
                    foreach (var exc in lastExcercises)
                    {
                        if (!ExcerciseSolutionStatus(exc, userID) && !IssueSolutionStatus(GetExcerciseIssueNumber(exc, userID), userID))
                        {
                            return exc;
                        }
                    }

                    // gdy dojdzie się do końca, to wektor wiedzy się zeruje i zaczyna się od samego początku
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                       var user = db.Students.FirstOrDefault(ex => ex.Id == userID);
                       var solvedExcercises = user.ProperlySolvedExcercises.Where(ex => ex.graph.Id == user.ActualSubject.Id);
                        foreach(var ex in solvedExcercises)
                        {
                            user.ProperlySolvedExcercises.Remove(ex);
                        }

                        var solvedIssues = user.SolvedIssues.Where(ex => ex.graph.Id == user.ActualSubject.Id);
                        foreach (var ex in solvedIssues)
                        {
                            user.SolvedIssues.Remove(ex);
                        }

                        db.SaveChanges();
                    }

                    return GetExcerciseNumberFromIssue(GetIssuesCount(pojecia, 0), 0, pojecia, userID);

                 }

            }
        }

        public int GetNextExcerciseNumber(int excerciseNumber, bool answer, string userID)
        {
            int[,] pojecian;
            int[,] pojeciap;
            int[,] zadania;

            GetExcerciseOrder(out pojecian, out pojeciap, out zadania, userID);

            if (!ExcerciseSolutionStatus(excerciseNumber, userID) && !IssueSolutionStatus(GetExcerciseIssueNumber(excerciseNumber, userID), userID))
                if (answer == true)
                {
                    SaveSolvedExcercise(excerciseNumber, userID);
                }

            // szukanie w którym pojęciu znajduje się wykonane zadanie
            // jeżeli nr zadania będzie spoza zakresu 1 - 46, to wylosuje się zadanie dla pojęcia pierwszego

            int start = GetExcerciseIssueNumber(excerciseNumber, userID);
            int iloscz = 0;

                if (answer == true)
                {
                    iloscz = GetIssuesCount(pojecian, start);

                    if (iloscz != 0)
                    {
                        return GetExcerciseNumberFromIssue(iloscz, start, pojecian, userID);
                    }

                    //jesli nie ma to idz do nastepnego w kolejnosci
                    else
                    {
                        int zadanie = 0;
                        while (zadanie == 0)
                        {
                            start = start + 1;
                            iloscz = GetIssuesCount(pojecian, start);
                            zadanie = GetExcerciseNumberFromIssue(iloscz, start, pojecian, userID);
                        }
                        return zadanie;
                    }

                }
                // jeśli odpowiedziano niepoprawnie
                else
                {
                    if (excerciseNumber == 1)
                        return 1;

                    iloscz = GetIssuesCount(pojeciap, start);

                    if (iloscz != 0)
                    {
                        return GetExcerciseNumberFromIssue(iloscz, start, pojeciap, userID);
                    }

                    //jesli nie ma  to idz do nastepnego (następnego, z listy następników...) w kolejnosci
                    else
                    {
                        if (start == 0)
                        {
                            return GetExcerciseNumberFromIssue(iloscz, start, pojecian, userID);
                        }

                        else
                        {
                            int zadanie = 0;
                            while (zadanie == 0)
                            {
                                start = start - 1;
                                zadanie = GetExcerciseNumberFromIssue(iloscz, start, pojecian, userID);
                            }
                            return zadanie;
                        }
                    }
             }
         }  
    }
}
