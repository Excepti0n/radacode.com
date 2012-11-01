namespace RadaCode.Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomerName = c.String(),
                        CustomerCompanySize = c.String(),
                        NetRevenue = c.String(),
                        WebSiteUrl = c.String(),
                        Industry_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Industries", t => t.Industry_Id)
                .Index(t => t.Industry_Id);
            
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SoftwareProjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DateStarted = c.DateTime(),
                        ProjectEstimate = c.Time(nullable: false),
                        ProjectActualCompletionSpan = c.Time(nullable: false),
                        DateFinished = c.DateTime(nullable: false),
                        WebSiteUrl = c.String(),
                        CurrentUsersCount = c.Int(nullable: false),
                        ROIpercentage = c.Int(nullable: false),
                        IsCloudConnected = c.Boolean(nullable: false),
                        ProjectDescriptionMarkup = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Customer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SoftwareProjects", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", new[] { "Industry_Id" });
            DropForeignKey("dbo.SoftwareProjects", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Customers", "Industry_Id", "dbo.Industries");
            DropTable("dbo.SoftwareProjects");
            DropTable("dbo.Industries");
            DropTable("dbo.Customers");
        }
    }
}
