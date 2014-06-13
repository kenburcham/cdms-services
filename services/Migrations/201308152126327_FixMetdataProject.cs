namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMetdataProject : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MetadataValues", "Project_Id", "dbo.Projects");
            DropIndex("dbo.MetadataValues", new[] { "Project_Id" });
            DropColumn("dbo.MetadataValues", "Project_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MetadataValues", "Project_Id", c => c.Int());
            CreateIndex("dbo.MetadataValues", "Project_Id");
            AddForeignKey("dbo.MetadataValues", "Project_Id", "dbo.Projects", "Id");
        }
    }
}
