using System;
using System.Collections.Generic;
using ZSPD.Domain.Models.EntityModels.Accounts;
using ZSPD.Domain.Models.EntityModels;
using System.Linq;
using System.IO;

using ZSPD.Domain.Models;
using System.Reflection;

namespace ZSPD.Domain.Managers
{
    public class StudentManager : IStudentManager
    {

        int[,] pojecian = new int[23, 4];
        int[,] pojeciap = new int[23, 2];
        int[,] zadania = new int[23, 3];

        public StudentManager()
        {
            GetExcerciseOrder();    // Czy potrzebujemy tego cały czas? Każde wywołanie StudentManager'a będzie wywołowało ten konstruktor
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

        public void GetExcerciseOrder()
        {
            // wczytanie z pliku tekstowego wartosci dla nastepnikow
            string poj = System.IO.File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"bin", "Lista_pojec.txt"));
            // wczytanie z pliku tekstowego wartosci dla poprzednikow
            string poj1 = System.IO.File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Lista_pojec1.txt"));
            // wczytanie z pliku tekstowego zadan dla odp pojec
            string zad = System.IO.File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Lista_zadan.txt"));

            string[] roz = poj.Split(';');
            string[] split = poj1.Split(';');
            string[] dew = zad.Split(';');

            // wczytywanie listy nastepnikow do tablicy
            for (int i = 0; i < roz.Length; i++)
            {
                string[] roz1 = roz[i].Split(',');
                for (int j = 0; j < roz1.Length; j++)
                {

                    pojecian[i, j] = int.Parse(roz1[j]);
                }
            }

            // wczytywanie listy poprzednikow do tablicy
            for (int i = 0; i < split.Length; i++)
            {
                string[] split1 = split[i].Split(',');
                for (int j = 0; j < split1.Length; j++)
                {

                    pojeciap[i, j] = int.Parse(split1[j]);
                }
            }

            //wczytywanie listy zadan do tablicy
            for (int i = 0; i < dew.Length; i++)
            {
                string[] dew1 = dew[i].Split(',');
                for (int j = 0; j < dew1.Length; j++)
                {

                    zadania[i, j] = int.Parse(dew1[j]);
                }
            }
        }

        public int GetStudentAdvacement(int advacement)
        {
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

            return zadania[sectionNumber, excercisePosition];

        }

        public int GetNextExcerciseNumber(int excerciseNumber, bool answer)
        {
            System.Random x = new Random();
            System.Random nastepnik = new Random();
            System.Random nrzadania = new Random();
            System.Random kierunek = new Random();

            // szukanie w którym pojęciu znajduje się wykonane zadanie
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

            //*****// BRAKUJE ZADANIA NR 16! I faktycznie program się zapętla po 
            if (pojecie == 16)
                pojecie = 17;
            //*****//

            int start = pojecie;
            int ilosc = 0;
            int iloscz = 0;

                if (answer == true)
                {
                    //zerujemy ilosci
                    ilosc = 0;
                    iloscz = 0;

                    // sprawdzenie czy jest nastepne pojecie 
                    while (pojecian[start, ilosc] != 0 && iloscz < 4)
                    {
                        iloscz++;

                        if (ilosc < 3)
                        {
                            ilosc++;
                        }
                    }

                    if (iloscz != 0)
                    {
                        int wylosowana = nastepnik.Next(1, iloscz + 1);
                        int wylosowana1 = pojecian[start, wylosowana - 1];
                        return wylosowana1;
                    }

                    //jesli nie ma  to idz do nastepnego w kolejnosci
                    else
                    {
                        start = start + 1;
                        int wylosowana = nastepnik.Next(1, iloscz + 1);
                        int wylosowana1 = pojecian[start, wylosowana - 1];
                        return wylosowana1;
                    }

                }

                else
                {
                    //zerujemy ilosci
                    ilosc = 0;
                    iloscz = 0;

                    // sprawdzenie czy jest nastepne (poprzednie, z listy poprzedników...) pojecie 
                    while (pojeciap[start, ilosc] != 0 && iloscz < 1)
                    {
                        iloscz++;

                        if (ilosc < 3)
                        {
                            ilosc++;
                        }
                    }

                    if (iloscz != 0)
                    {
                        int wylosowana = nastepnik.Next(1, iloscz + 1);
                        int wylosowana1 = pojeciap[start, wylosowana - 1];
                        return wylosowana1;
                    }

                //jesli nie ma  to idz do nastepnego (następnego, z listy następników...) w kolejnosci
                    else
                    {
                        if (start == 0)
                        {
                            int wylosowana = nastepnik.Next(1, iloscz + 1);
                            int wylosowana1 = pojecian[start, wylosowana - 1];
                            return wylosowana1;
                        }
                        else
                       {
                            start = start - 1;
                            int wylosowana = nastepnik.Next(1, iloscz + 1);
                            int wylosowana1 = pojecian[start, wylosowana - 1];
                            return wylosowana1;
                        }

                    }

             }
         }  
    }
}
