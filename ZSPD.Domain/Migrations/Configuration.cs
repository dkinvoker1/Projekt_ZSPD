namespace ZSPD.Domain.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
    using ZSPD.Domain.Models.EntityModels;
    using ZSPD.Domain.Models.EntityModels.Accounts;

    internal sealed class Configuration : DbMigrationsConfiguration<ZSPD.Domain.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ZSPD.Domain.Models.ApplicationDbContext";
        }

        protected override void Seed(ZSPD.Domain.Models.ApplicationDbContext context)
        {
            CreateRoles(context);

            CreateSubject(context);
            AddUsers(context);
            AddQuestions(context);
            AddPsychologists(context);


            var userStore = new UserStore<AppUser>(context);
            var userManager = new UserManager<AppUser>(userStore);
            if (!(context.Users.Any(u => u.UserName == "admin")))
            {
                var userToInsert = new AppUser
                {
                    UserName = "admin",
                };
                userManager.Create(userToInsert, "superTajneHas³o123");
                userManager.AddToRole(userToInsert.Id, Roles.Admin);
            }
        }

        private string MapPath(string seedFile)
        {
            if (HttpContext.Current != null)
                return HostingEnvironment.MapPath(seedFile);

            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath; //was AbsolutePath but didn't work with spaces according to comments
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, "..\\.." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }

        private void CreateSubject(ZSPD.Domain.Models.ApplicationDbContext context)
        {
            if (context.SubjectGraphs.Count() == 0)
            {
                // wczytanie z pliku tekstowego wartosci dla nastepnikow
                string poj = File.ReadAllText(MapPath("~/Resources/Lista_pojec.txt")); // Cofniêcie o 1 folder w drzewku
                                                                                                 // wczytanie z pliku tekstowego wartosci dla poprzednikow
                string poj1 = File.ReadAllText(MapPath("~/Resources/Lista_pojec1.txt"));
                // wczytanie z pliku tekstowego zadan dla odp pojec
                string zad = File.ReadAllText(MapPath("~/Resources/Lista_zadan.txt"));

                SubjectGraph graph = new SubjectGraph()
                {
                    Name = "Algebra",
                    NextConcepts = poj,
                    PreviousConcepts = poj1,
                    Exercises = zad
                };

                context.SubjectGraphs.Add(graph);
                context.SaveChanges();
            }
        }

        private void CreateRoles(ZSPD.Domain.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<Student>(new UserStore<Student>(context));


            if (!roleManager.RoleExists(Roles.User))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = Roles.User
                };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists(Roles.Psychologist))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = Roles.Psychologist
                };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists(Roles.Judge))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = Roles.Judge
                };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists(Roles.Admin))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = Roles.Admin,
                };
                roleManager.Create(role);
            }
        }

        private void AddUsers(ZSPD.Domain.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<Student>(context);
            var userManager = new UserManager<Student>(userStore);

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            if (!(context.Users.Any(u => u.UserName == "testUser")))
            {
                var userToInsert = new Student { UserName = "testUser", ActualSubject = context.SubjectGraphs.First() };

                userManager.Create(userToInsert, "test");
                userManager.AddToRole(userToInsert.Id, Roles.User);
            }

            if (!(context.Users.Any(u => u.UserName == "testUser2")))
            {
                var userToInsert = new Student { UserName = "testUser2", ActualSubject = context.SubjectGraphs.First() };

                userManager.Create(userToInsert, "test");
                userManager.AddToRole(userToInsert.Id, Roles.User);
            }

            context.SaveChanges();
        }
        private void AddPsychologists(ZSPD.Domain.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<Psychologist>(context);
            var userManager = new UserManager<Psychologist>(userStore);

            if (!(context.Users.Any(u => u.UserName == "testPsychologist")))
            {
                var userToInsert = new Psychologist
                {
                    UserName = "testPsychologist",
                    Name = "TestName",
                    Surname = "TestSurname",
                };
                userManager.Create(userToInsert, "testPsychologist!");
                userManager.AddToRole(userToInsert.Id, Roles.Psychologist);
            }


            if (!(context.Users.Any(u => u.UserName == "testPsychologist2")))
            {
                var userToInsert = new Psychologist
                {
                    UserName = "testPsychologist2",
                    Name = "TestName2",
                    Surname = "TestSurname2",
                };
                userManager.Create(userToInsert, "testPsychologist!");
                userManager.AddToRole(userToInsert.Id, Roles.Psychologist);
            }
            context.SaveChanges();
        }
        private void AddQuestions(ZSPD.Domain.Models.ApplicationDbContext context)
        {
            if (context.Questions.Count() < 2)
            {
                context.Questions.AddRange(
                    new List<Question>(){
                        new Question() { Content = "Pierwsze pytanie testowe" },
                        new Question() { Content = "Drugie pytanie testowe", }
                    }
                );
            }
            context.SaveChanges();
        }


    }
}
