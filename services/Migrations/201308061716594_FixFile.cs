namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixFile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "FileType_Id", "dbo.FileTypes");
            DropIndex("dbo.Files", new[] { "FileType_Id" });
            RenameColumn(table: "dbo.Files", name: "FileType_Id", newName: "FileTypeId");
            AddForeignKey("dbo.Files", "FileTypeId", "dbo.FileTypes", "Id", cascadeDelete: true);
            CreateIndex("dbo.Files", "FileTypeId");
            DropColumn("dbo.Files", "TypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "TypeId", c => c.String());
            DropIndex("dbo.Files", new[] { "FileTypeId" });
            DropForeignKey("dbo.Files", "FileTypeId", "dbo.FileTypes");
            RenameColumn(table: "dbo.Files", name: "FileTypeId", newName: "FileType_Id");
            CreateIndex("dbo.Files", "FileType_Id");
            AddForeignKey("dbo.Files", "FileType_Id", "dbo.FileTypes", "Id");
        }
    }
}
