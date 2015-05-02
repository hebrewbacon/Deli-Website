namespace ItalianDeli.Migrations
{
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Web;
    using System.Data.Entity.Migrations;
    using ItalianDeli.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ItalianDeli.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ItalianDeli.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var roleCook = new RoleManager<IdentityRole>(roleStore);
            var roleDelivery = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = new ApplicationUser { UserName = "admin@gmail.com" };
            var cook = new ApplicationUser { UserName = "cook@gmail.com" };
            var delivery = new ApplicationUser { UserName = "delivery@gmail.com" };
            var guestUser = new ApplicationUser { UserName = "guest@guest.com" };

            userManager.Create(user, "abc123"); //strong password!#@$!
            userManager.Create(guestUser, "guest1"); //strong password!#@$!
            userManager.Create(cook, "abc123");
            userManager.Create(delivery, "abc123");

            roleManager.Create(new IdentityRole { Name = "Admin" });
            userManager.AddToRole(user.Id, "Admin");

            roleCook.Create(new IdentityRole { Name = "Cook" });
            userManager.AddToRole(cook.Id, "Cook");

            roleCook.Create(new IdentityRole { Name = "Delivery" });
            userManager.AddToRole(cook.Id, "Delivery");
        }
    }
}
