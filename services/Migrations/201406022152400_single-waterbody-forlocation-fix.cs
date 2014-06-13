namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class singlewaterbodyforlocationfix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WaterBodies", "Location_Id", "dbo.Locations");
            DropIndex("dbo.WaterBodies", new[] { "Location_Id" });
            AddColumn("dbo.Locations", "WaterBodyId", c => c.Int());
            AddForeignKey("dbo.Locations", "WaterBodyId", "dbo.WaterBodies", "Id");
            CreateIndex("dbo.Locations", "WaterBodyId");
            DropColumn("dbo.WaterBodies", "Location_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WaterBodies", "Location_Id", c => c.Int());
            DropIndex("dbo.Locations", new[] { "WaterBodyId" });
            DropForeignKey("dbo.Locations", "WaterBodyId", "dbo.WaterBodies");
            DropColumn("dbo.Locations", "WaterBodyId");
            CreateIndex("dbo.WaterBodies", "Location_Id");
            AddForeignKey("dbo.WaterBodies", "Location_Id", "dbo.Locations", "Id");
        }
    }
}
