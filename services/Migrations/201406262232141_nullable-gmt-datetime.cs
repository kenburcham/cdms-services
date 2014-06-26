namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullablegmtdatetime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WaterTemp_Detail", "GMTReadingDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WaterTemp_Detail", "GMTReadingDateTime", c => c.DateTime(nullable: false));
        }
    }
}
