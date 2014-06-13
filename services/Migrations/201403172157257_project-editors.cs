namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projecteditors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.User_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: false)
                .Index(t => t.Project_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProjectUsers", new[] { "User_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "Project_Id" });
            DropForeignKey("dbo.ProjectUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ProjectUsers", "Project_Id", "dbo.Projects");
            DropTable("dbo.ProjectUsers");
        }
    }
}
