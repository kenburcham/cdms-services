namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class singlewaterbodiesforlocations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WaterBodyLocations", "WaterBody_Id", "dbo.WaterBodies");
            DropForeignKey("dbo.WaterBodyLocations", "Location_Id", "dbo.Locations");
            DropIndex("dbo.WaterBodyLocations", new[] { "WaterBody_Id" });
            DropIndex("dbo.WaterBodyLocations", new[] { "Location_Id" });
            AddColumn("dbo.WaterBodies", "Location_Id", c => c.Int());
            AddForeignKey("dbo.WaterBodies", "Location_Id", "dbo.Locations", "Id");
            CreateIndex("dbo.WaterBodies", "Location_Id");
            DropColumn("dbo.WaterBodies", "SdeObjectId");
            DropTable("dbo.WaterBodyLocations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WaterBodyLocations",
                c => new
                    {
                        WaterBody_Id = c.Int(nullable: false),
                        Location_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WaterBody_Id, t.Location_Id });
            
            AddColumn("dbo.WaterBodies", "SdeObjectId", c => c.Int());
            DropIndex("dbo.WaterBodies", new[] { "Location_Id" });
            DropForeignKey("dbo.WaterBodies", "Location_Id", "dbo.Locations");
            DropColumn("dbo.WaterBodies", "Location_Id");
            CreateIndex("dbo.WaterBodyLocations", "Location_Id");
            CreateIndex("dbo.WaterBodyLocations", "WaterBody_Id");
            AddForeignKey("dbo.WaterBodyLocations", "Location_Id", "dbo.Locations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.WaterBodyLocations", "WaterBody_Id", "dbo.WaterBodies", "Id", cascadeDelete: true);
        }
    }
}
