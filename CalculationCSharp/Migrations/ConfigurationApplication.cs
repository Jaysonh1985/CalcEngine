namespace CalculationCSharp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class configurationApplication : DbMigrationsConfiguration<CalculationCSharp.Models.ApplicationDbContext>
    {
        public configurationApplication()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "CalculationCSharp.Models.ApplicationDbContext";
            Database.SetInitializer(new CreateDatabaseIfNotExists<CalculationCSharp.Models.ApplicationDbContext>());
        }

        protected override void Seed(CalculationCSharp.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(RoleNameModels.ROLE_ADMINISTRATOR))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNameModels.ROLE_ADMINISTRATOR));
            }
            if (!roleManager.RoleExists(RoleNameModels.ROLE_CONFIGURATION))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNameModels.ROLE_CONFIGURATION));
            }
            if (!roleManager.RoleExists(RoleNameModels.ROLE_VIEWONLY))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNameModels.ROLE_VIEWONLY));
            }
            if (!roleManager.RoleExists(RoleNameModels.ROLE_SYSADMIN))
            {
                var roleresult = roleManager.Create(new IdentityRole(RoleNameModels.ROLE_SYSADMIN));
            }

            string userName = "system@projectAim.com";
            string password = "Password01*";

            ApplicationUser user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };
                IdentityResult userResult = userManager.Create(user, password);
                if (userResult.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, RoleNameModels.ROLE_SYSADMIN);
                }
            }

        }
    }
}
