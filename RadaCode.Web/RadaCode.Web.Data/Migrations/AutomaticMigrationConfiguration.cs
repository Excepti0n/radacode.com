using System.Collections.Generic;
using System.Data.SqlTypes;
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
            var roles = new List<WebUserRole>
                {
                    new WebUserRole()
                        {
                            Id = Guid.Parse("9727d3e4-0269-46e1-ad7c-bfbdc9c074bc"),
                            RoleName = "Admin"
                        },
                    new WebUserRole()
                        {
                            Id = Guid.Parse("b22380eb-1c28-4945-b12d-38c55099036a"),
                            RoleName = "ProjectsManager"
                        }
                };

            foreach (var webUserRole in roles.Where(webUserRole => !context.WebUserRoles.Any(rl => rl.RoleName == webUserRole.RoleName)))
            {
                context.WebUserRoles.Add(webUserRole);
            }

            var mp = new WebUser()
                {
                    Id = Guid.Parse("479460f6-06c1-43fb-96c3-6ff161255c04"),
                    CreateDate = DateTime.Now,
                    UserName = "MaxPavlov",
                    Password = Crypto.HashPassword("jackPecker"),
                    Roles = context.WebUserRoles.Local.Where(rl => rl.RoleName == "Admin").ToList(),
                    IsLockedOut = false,
                    LastActivityDate = SqlDateTime.MinValue.Value,
                    LastLoginDate = SqlDateTime.MinValue.Value,
                    LastLockoutDate = SqlDateTime.MinValue.Value,
                    LastPasswordChangedDate = SqlDateTime.MinValue.Value,
                    LastPasswordFailureDate = SqlDateTime.MinValue.Value,
                    PasswordVerificationTokenExpirationDate = SqlDateTime.MinValue.Value
                };

            mp.Roles.Add(context.WebUserRoles.First(rl => rl.RoleName == "Admin"));


            if (!context.WebUsers.Any(usr => usr.UserName == mp.UserName)) context.WebUsers.Add(mp);



            base.Seed(context);

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
