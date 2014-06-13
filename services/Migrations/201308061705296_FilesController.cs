namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilesController : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                        Size = c.String(),
                        Link = c.String(),
                        TypeId = c.String(),
                        FileType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.FileTypes", t => t.FileType_Id)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId)
                .Index(t => t.FileType_Id);
            
            CreateTable(
                "dbo.FileTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Files", new[] { "FileType_Id" });
            DropIndex("dbo.Files", new[] { "UserId" });
            DropIndex("dbo.Files", new[] { "ProjectId" });
            DropForeignKey("dbo.Files", "FileType_Id", "dbo.FileTypes");
            DropForeignKey("dbo.Files", "UserId", "dbo.Users");
            DropForeignKey("dbo.Files", "ProjectId", "dbo.Projects");
            DropTable("dbo.FileTypes");
            DropTable("dbo.Files");
        }
    }
}
