namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtempFfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Header", "AirTemperatureF", c => c.Double());
            AddColumn("dbo.AdultWeir_Header", "WaterTemperatureF", c => c.Double());
            AddColumn("dbo.AdultWeir_Header", "TransportReleaseTemperatureF", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdultWeir_Header", "TransportReleaseTemperatureF");
            DropColumn("dbo.AdultWeir_Header", "WaterTemperatureF");
            DropColumn("dbo.AdultWeir_Header", "AirTemperatureF");
        }
    }
}
