namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatewatertempfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterTemp_Detail", "DepthToWater", c => c.Double());
            DropColumn("dbo.WaterTemp_Header", "TimeStart");
            DropColumn("dbo.WaterTemp_Header", "TimeEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WaterTemp_Header", "TimeEnd", c => c.String());
            AddColumn("dbo.WaterTemp_Header", "TimeStart", c => c.String());
            DropColumn("dbo.WaterTemp_Detail", "DepthToWater");
        }
    }
}
