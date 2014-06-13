namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectDatesCanBeNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "StartDate", c => c.DateTime());
            AlterColumn("dbo.Projects", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
