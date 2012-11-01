using System.Data.Entity;
using RadaCode.Web.Data.Entities;

namespace RadaCode.Web.Data.EF
{
    public class RadaCodeWebStoreContext : DbContext
    {
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<WebUserRole> WebUserRoles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<SoftwareProject> SoftwareProjects { get; set; } 
    }
}
