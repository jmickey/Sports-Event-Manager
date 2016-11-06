using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SportsManager.Models;

[assembly: OwinStartupAttribute(typeof(SportsManager.Startup))]
namespace SportsManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createUsersAndRoles();
        }

        private void createUsersAndRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // Check if the admin role exists, if not create it and the admin user   
            if (!roleManager.RoleExists("Admin"))
            {
                // Create the admin role  
                var adminRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                adminRole.Name = "Admin";
                roleManager.Create(adminRole);
            }

            var admin = userManager.FindByEmail("admin@ecu.com");

            if (admin == null) {
                // Create the admin user
                var adminUser = new ApplicationUser();
                adminUser.Email = "admin@ecu.com";
                adminUser.UserName = adminUser.Email;
                string adminPassword = "Admin#1";

                var createAdmin = userManager.Create(adminUser, adminPassword);

                // Add the admin user to the admin role
                if (createAdmin.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }
            }

            // Check if the event manager role exists, if not create it and the event manager user
            if (!roleManager.RoleExists("EventManager"))
            {
                // Create the event manager role
                var eventManagerRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                eventManagerRole.Name = "EventManager";
                roleManager.Create(eventManagerRole);
            }

            var eventManager = userManager.FindByEmail("event.manager@ecu.edu.au");

            if (eventManager == null)
            {
                // Create the event manager user
                var eventManagerUser = new ApplicationUser();
                eventManagerUser.Email = "event@ecu.com";
                eventManagerUser.UserName = eventManagerUser.Email;
                string eventManagerPassword = "Event#1";

                var createEventManager = userManager.Create(eventManagerUser, eventManagerPassword);

                // Add the event manager user to the event manager role
                if (createEventManager.Succeeded)
                {
                    userManager.AddToRole(eventManagerUser.Id, "EventManager");
                }
            }
        }
    }
}
