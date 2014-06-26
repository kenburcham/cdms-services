namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gmtreadingdatetimetowatertemp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterTemp_Detail", "GMTReadingDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterTemp_Detail", "GMTReadingDateTime");
        }
    }
}
