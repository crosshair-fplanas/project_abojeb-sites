using AbojebApi.Core.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AbojebApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DbConnectionString", throwIfV1Schema: false)
        {
            Database.SetInitializer(new UserInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<AbojebApi.Models.ApplicationUser> ApplicationUsers { get; set; }
    }

    public class UserInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Initialize default identity roles
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            List<IdentityRole> identityRoles = new List<IdentityRole>();
            identityRoles.Add(new IdentityRole() { Name = RoleTypes.Administrator.ToString() });
            identityRoles.Add(new IdentityRole() { Name = RoleTypes.User.ToString() });

            foreach (IdentityRole role in identityRoles)
            {
                roleManager.Create(role);
            }

            // Initialize default user
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser admin = new ApplicationUser () {
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Crosstech",
                Email = "admin@crosstech-solutions.com"
            };

            userManager.Create(admin, "P@ssw0rd@05");
            userManager.AddToRole(admin.Id, RoleTypes.Administrator.ToString()); 

            base.Seed(context);
        }
    }
}