namespace RadaCode.Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        LastPasswordFailureDate = c.DateTime(),
                        LastActivityDate = c.DateTime(),
                        LastLockoutDate = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                        ConfirmationToken = c.String(),
                        CreateDate = c.DateTime(),
                        IsLockedOut = c.Boolean(nullable: false),
                        LastPasswordChangedDate = c.DateTime(),
                        PasswordVerificationToken = c.String(),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebUserRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebUserRoleWebUsers",
                c => new
                    {
                        WebUserRole_Id = c.Guid(nullable: false),
                        WebUser_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.WebUserRole_Id, t.WebUser_Id })
                .ForeignKey("dbo.WebUserRoles", t => t.WebUserRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.WebUsers", t => t.WebUser_Id, cascadeDelete: true)
                .Index(t => t.WebUserRole_Id)
                .Index(t => t.WebUser_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.WebUserRoleWebUsers", new[] { "WebUser_Id" });
            DropIndex("dbo.WebUserRoleWebUsers", new[] { "WebUserRole_Id" });
            DropForeignKey("dbo.WebUserRoleWebUsers", "WebUser_Id", "dbo.WebUsers");
            DropForeignKey("dbo.WebUserRoleWebUsers", "WebUserRole_Id", "dbo.WebUserRoles");
            DropTable("dbo.WebUserRoleWebUsers");
            DropTable("dbo.WebUserRoles");
            DropTable("dbo.WebUsers");
        }
    }
}
