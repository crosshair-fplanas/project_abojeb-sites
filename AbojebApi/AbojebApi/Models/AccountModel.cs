using AbojebApi.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AbojebApi.Models
{
    public class AccountModel
    {
        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var result = await userManager.Users.ToListAsync();
                return result;
            }
        }
    }
}