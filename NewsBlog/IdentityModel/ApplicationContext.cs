using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NewsBlog.IdentityModel
{

    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("IdentityDb") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
    }
}