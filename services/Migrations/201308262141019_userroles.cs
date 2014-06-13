namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userroles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Users", "OrganizationId", "dbo.Organizations");
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.Users", new[] { "OrganizationId" });
            AddColumn("dbo.Users", "Roles", c => c.String());
            AlterColumn("dbo.Users", "OrganizationId", c => c.Int());
            AlterColumn("dbo.Users", "DepartmentId", c => c.Int());
            AddForeignKey("dbo.Users", "DepartmentId", "dbo.Departments", "Id");
            AddForeignKey("dbo.Users", "OrganizationId", "dbo.Organizations", "Id");
            CreateIndex("dbo.Users", "DepartmentId");
            CreateIndex("dbo.Users", "OrganizationId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "OrganizationId" });
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropForeignKey("dbo.Users", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            AlterColumn("dbo.Users", "DepartmentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "OrganizationId", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "Roles");
            CreateIndex("dbo.Users", "OrganizationId");
            CreateIndex("dbo.Users", "DepartmentId");
            AddForeignKey("dbo.Users", "OrganizationId", "dbo.Organizations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Users", "DepartmentId", "dbo.Departments", "Id", cascadeDelete: true);
        }
    }
}
