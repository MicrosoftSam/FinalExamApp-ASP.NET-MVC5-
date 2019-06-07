using AlgebraApp1._1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AlgebraApp1._1.Startup))]
namespace AlgebraApp1._1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole("Admin");
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "bigboss@algebra.hr";
                user.Email = "bigboss@algebra.hr";

                string userPWD = "BigBoss_666";

                var chkUser = userManager.Create(user, userPWD);

                if (chkUser.Succeeded)
                {
                    var userResult = userManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole("Employee");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole("User");
                roleManager.Create(role);
            }
        }

    }
}
