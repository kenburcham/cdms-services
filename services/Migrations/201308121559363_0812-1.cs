namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _08121 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetadataValues", "Project_Id", c => c.Int());
            AddForeignKey("dbo.MetadataValues", "Project_Id", "dbo.Projects", "Id");
            CreateIndex("dbo.MetadataValues", "Project_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.MetadataValues", new[] { "Project_Id" });
            DropForeignKey("dbo.MetadataValues", "Project_Id", "dbo.Projects");
            DropColumn("dbo.MetadataValues", "Project_Id");
        }
    }
}
