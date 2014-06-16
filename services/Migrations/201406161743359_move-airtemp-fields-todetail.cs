namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moveairtempfieldstodetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterTemp_Detail", "AirTemperature", c => c.Double());
            AddColumn("dbo.WaterTemp_Detail", "AirTemperatureF", c => c.Double());
            DropColumn("dbo.WaterTemp_Header", "AirTemperature");
            DropColumn("dbo.WaterTemp_Header", "AirTemperatureF");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WaterTemp_Header", "AirTemperatureF", c => c.Double());
            AddColumn("dbo.WaterTemp_Header", "AirTemperature", c => c.Double());
            DropColumn("dbo.WaterTemp_Detail", "AirTemperatureF");
            DropColumn("dbo.WaterTemp_Detail", "AirTemperature");
        }
    }
}
