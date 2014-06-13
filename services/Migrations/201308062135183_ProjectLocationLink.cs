namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectLocationLink : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Projects", "ProjectLocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "ProjectLocationId", c => c.Int(nullable: false));
        }
    }
}
