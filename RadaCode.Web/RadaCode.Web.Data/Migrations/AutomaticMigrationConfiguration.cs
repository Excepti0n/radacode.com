using System.Collections.Generic;
using RadaCode.Web.Data.Entities;
using RadaCode.Web.Data.Utils;

namespace RadaCode.Web.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class AutomaticMigrationConfiguration : DbMigrationsConfiguration<RadaCode.Web.Data.EF.RadaCodeWebStoreContext>
    {
        public AutomaticMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(RadaCode.Web.Data.EF.RadaCodeWebStoreContext context)
        {
            new List<WebUserRole>
            {
                new WebUserRole() { RoleName = "Admin"
                         },
                new WebUserRole() { RoleName = "ProjectsManager"}
            }.ForEach(r => context.WebUserRoles.AddOrUpdate(r));

            var mp = new WebUser()
                {
                    CreateDate = DateTime.Now,
                    UserName = "MaxPavlov",
                    Password = Crypto.HashPassword("jackPecker"),
                    Roles = context.WebUserRoles.Local.Where(rl => rl.RoleName == "Admin").ToList()
                };

            context.WebUsers.AddOrUpdate(mp);

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
