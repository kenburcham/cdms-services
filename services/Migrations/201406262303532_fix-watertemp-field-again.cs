namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixwatertempfieldagain : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.WaterTemp_Detail", "DepthToWater");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WaterTemp_Detail", "DepthToWater", c => c.Double());
        }
    }
}
