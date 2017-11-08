using System;
using Microsoft.Owin;
using Owin;
using ZSPD.Domain.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ZSPD.Domain.Models.EntityModels.Accounts;

[assembly: OwinStartupAttribute(typeof(ZSPD.App_Start.Startup))]
namespace ZSPD.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}