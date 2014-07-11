namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scottodwatertempfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterTemp_Detail", "Conductivity", c => c.Double());
            AddColumn("dbo.WaterTemp_Header", "DeployTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterTemp_Header", "DeployTime");
            DropColumn("dbo.WaterTemp_Detail", "Conductivity");
        }
    }
}
