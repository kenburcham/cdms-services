namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class watertemp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WaterBodies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        SdeObjectId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InstrumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WaterTemp_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReadingDateTime = c.DateTime(nullable: false),
                        WaterTemperature = c.Double(nullable: false),
                        WaterTemperatureF = c.Double(nullable: false),
                        WaterLevel = c.Double(nullable: false),
                        TempAToD = c.Double(nullable: false),
                        BatteryVolts = c.Double(),
                        RowId = c.Int(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .ForeignKey("dbo.QAStatus", t => t.QAStatusId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId)
                .Index(t => t.QAStatusId);
            
            CreateTable(
                "dbo.WaterTemp_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AirTemperature = c.Double(),
                        AirTemperatureF = c.Double(),
                        TimeStart = c.String(),
                        TimeEnd = c.String(),
                        Technicians = c.String(),
                        Comments = c.String(),
                        CollectionType = c.String(),
                        DepthToWater = c.Double(),
                        PSI = c.Double(),
                        StaticWaterLevel = c.Double(),
                        WeatherConditions = c.String(),
                        SamplePeriod = c.String(),
                        SampleTempUnit = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            CreateTable(
                "dbo.WaterBodyLocations",
                c => new
                    {
                        WaterBody_Id = c.Int(nullable: false),
                        Location_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WaterBody_Id, t.Location_Id })
                .ForeignKey("dbo.WaterBodies", t => t.WaterBody_Id, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_Id, cascadeDelete: true)
                .Index(t => t.WaterBody_Id)
                .Index(t => t.Location_Id);
            
            AddColumn("dbo.Locations", "Name", c => c.String());
            AddColumn("dbo.Locations", "Description", c => c.String());
            AddColumn("dbo.Locations", "CreateDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Locations", "UserId", c => c.Int());
            AddColumn("dbo.Locations", "Elevation", c => c.Int());
            AddColumn("dbo.Locations", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Locations", "GPSEasting", c => c.Decimal(precision: 18, scale: 8));
            AddColumn("dbo.Locations", "GPSNorthing", c => c.Decimal(precision: 18, scale: 8));
            AddColumn("dbo.Locations", "Projection", c => c.String());
            AddColumn("dbo.Locations", "UTMZone", c => c.String());
            AddColumn("dbo.Locations", "Latitude", c => c.Decimal(precision: 18, scale: 13));
            AddColumn("dbo.Locations", "Longitude", c => c.Decimal(precision: 18, scale: 13));
            AddColumn("dbo.Locations", "OtherAgencyId", c => c.String());
            AddColumn("dbo.Locations", "ImageLink", c => c.String());
            AddColumn("dbo.Locations", "WettedWidth", c => c.Single());
            AddColumn("dbo.Locations", "WettedDepth", c => c.Single());
            AddColumn("dbo.Locations", "RiverMile", c => c.Decimal(precision: 5, scale: 2));
            AddColumn("dbo.Instruments", "EnteredService", c => c.DateTime(nullable: false));
            AddColumn("dbo.Instruments", "EndedService", c => c.DateTime(nullable: false));
            AddColumn("dbo.Instruments", "DatastoreId", c => c.Int(nullable: false));
            AddColumn("dbo.Instruments", "InstrumentTypeId", c => c.String());
            AddColumn("dbo.Instruments", "DataQualityLevel", c => c.String());
            AddColumn("dbo.Instruments", "AccuracyCheckMethod", c => c.String());
            AddColumn("dbo.Instruments", "InstrumentType_Id", c => c.Int());
            AlterColumn("dbo.Locations", "SdeFeatureClassId", c => c.Int());
            AlterColumn("dbo.Locations", "SdeObjectId", c => c.Int());
            AddForeignKey("dbo.Instruments", "InstrumentType_Id", "dbo.InstrumentTypes", "Id");
            CreateIndex("dbo.Instruments", "InstrumentType_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WaterBodyLocations", new[] { "Location_Id" });
            DropIndex("dbo.WaterBodyLocations", new[] { "WaterBody_Id" });
            DropIndex("dbo.WaterTemp_Header", new[] { "ByUserId" });
            DropIndex("dbo.WaterTemp_Header", new[] { "ActivityId" });
            DropIndex("dbo.WaterTemp_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.WaterTemp_Detail", new[] { "ByUserId" });
            DropIndex("dbo.WaterTemp_Detail", new[] { "ActivityId" });
            DropIndex("dbo.Instruments", new[] { "InstrumentType_Id" });
            DropForeignKey("dbo.WaterBodyLocations", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.WaterBodyLocations", "WaterBody_Id", "dbo.WaterBodies");
            DropForeignKey("dbo.WaterTemp_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.WaterTemp_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.WaterTemp_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.WaterTemp_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.WaterTemp_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Instruments", "InstrumentType_Id", "dbo.InstrumentTypes");
            AlterColumn("dbo.Locations", "SdeObjectId", c => c.Int(nullable: false));
            AlterColumn("dbo.Locations", "SdeFeatureClassId", c => c.Int(nullable: false));
            DropColumn("dbo.Instruments", "InstrumentType_Id");
            DropColumn("dbo.Instruments", "AccuracyCheckMethod");
            DropColumn("dbo.Instruments", "DataQualityLevel");
            DropColumn("dbo.Instruments", "InstrumentTypeId");
            DropColumn("dbo.Instruments", "DatastoreId");
            DropColumn("dbo.Instruments", "EndedService");
            DropColumn("dbo.Instruments", "EnteredService");
            DropColumn("dbo.Locations", "RiverMile");
            DropColumn("dbo.Locations", "WettedDepth");
            DropColumn("dbo.Locations", "WettedWidth");
            DropColumn("dbo.Locations", "ImageLink");
            DropColumn("dbo.Locations", "OtherAgencyId");
            DropColumn("dbo.Locations", "Longitude");
            DropColumn("dbo.Locations", "Latitude");
            DropColumn("dbo.Locations", "UTMZone");
            DropColumn("dbo.Locations", "Projection");
            DropColumn("dbo.Locations", "GPSNorthing");
            DropColumn("dbo.Locations", "GPSEasting");
            DropColumn("dbo.Locations", "Status");
            DropColumn("dbo.Locations", "Elevation");
            DropColumn("dbo.Locations", "UserId");
            DropColumn("dbo.Locations", "CreateDateTime");
            DropColumn("dbo.Locations", "Description");
            DropColumn("dbo.Locations", "Name");
            DropTable("dbo.WaterBodyLocations");
            DropTable("dbo.WaterTemp_Header");
            DropTable("dbo.WaterTemp_Detail");
            DropTable("dbo.InstrumentTypes");
            DropTable("dbo.WaterBodies");
        }
    }
}
