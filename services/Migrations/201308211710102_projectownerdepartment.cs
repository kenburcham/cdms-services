namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projectownerdepartment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId);
            
            AddColumn("dbo.Projects", "OwnerId", c => c.Int(nullable: true));
            AddColumn("dbo.Users", "DepartmentId", c => c.Int(nullable: true));
            AddColumn("dbo.Users", "Fullname", c => c.String());
            AddForeignKey("dbo.Projects", "OwnerId", "dbo.Users", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Users", "DepartmentId", "dbo.Departments", "Id", cascadeDelete: false);
            CreateIndex("dbo.Projects", "OwnerId");
            CreateIndex("dbo.Users", "DepartmentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Departments", new[] { "OrganizationId" });
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.Projects", new[] { "OwnerId" });
            DropForeignKey("dbo.Departments", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Projects", "OwnerId", "dbo.Users");
            DropColumn("dbo.Users", "Fullname");
            DropColumn("dbo.Users", "DepartmentId");
            DropColumn("dbo.Projects", "OwnerId");
            DropTable("dbo.Departments");
        }
    }
}
