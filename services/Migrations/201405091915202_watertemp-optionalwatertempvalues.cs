namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class watertempoptionalwatertempvalues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WaterTemp_Detail", "WaterTemperature", c => c.Double());
            AlterColumn("dbo.WaterTemp_Detail", "WaterTemperatureF", c => c.Double());
            AlterColumn("dbo.WaterTemp_Detail", "WaterLevel", c => c.Double());
            AlterColumn("dbo.WaterTemp_Detail", "TempAToD", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WaterTemp_Detail", "TempAToD", c => c.Double(nullable: false));
            AlterColumn("dbo.WaterTemp_Detail", "WaterLevel", c => c.Double(nullable: false));
            AlterColumn("dbo.WaterTemp_Detail", "WaterTemperatureF", c => c.Double(nullable: false));
            AlterColumn("dbo.WaterTemp_Detail", "WaterTemperature", c => c.Double(nullable: false));
        }
    }
}
