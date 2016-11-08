using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;


namespace jmannionBlog.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }
            var userManager = new UserManager<Models.ApplicationUser>(
                new UserStore<Models.ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "john.aj.mannion@gmail"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "john.aj.mannion@gmail.com",
                    Email = "john.aj.mannion@gmail.com",

                    DisplayName = "john.aj.mannion@gmail.com"
                }, "True78&*");
            }
            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",

                    DisplayName = "moderator@coderfoundry.com"
                }, "Password-1");
            }

            var userId = userManager.FindByEmail("john.aj.mannion@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");
            var user_Id = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(user_Id, "Moderator");
        }
    }
}