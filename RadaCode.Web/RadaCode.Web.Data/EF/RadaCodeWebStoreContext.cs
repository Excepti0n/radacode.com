using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using RadaCode.Web.Data.Entities;
using RadaCode.Web.Data.Migrations;

namespace RadaCode.Web.Data.EF
{
    public class RadaCodeWebStoreContext : DbContext
    {
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<WebUserRole> WebUserRoles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<SoftwareProject> SoftwareProjects { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Tell Code First to ignore PluralizingTableName convention
            // If you keep this convention then the generated tables will have pluralized names.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //set the initializer to migration
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RadaCodeWebStoreContext, AutomaticMigrationConfiguration>());
        }
    }
}
