namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectOrgTweak : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Projects", "OrganizationId", "dbo.Organizations", "Id", cascadeDelete: true);
            CreateIndex("dbo.Projects", "OrganizationId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projects", new[] { "OrganizationId" });
            DropForeignKey("dbo.Projects", "OrganizationId", "dbo.Organizations");
        }
    }
}
