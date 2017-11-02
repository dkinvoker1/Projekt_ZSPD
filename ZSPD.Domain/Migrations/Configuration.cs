namespace ZSPD.Domain.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
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

            AddUsers(context);
            AddQuestions(context);
            AddPsychologists(context);
        }

        private void CreateRoles(ZSPD.Domain.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<User>(new UserStore<User>(context));


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
        }

        private void AddUsers(ZSPD.Domain.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

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
                var userToInsert = new User { UserName = "testUser" };

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
