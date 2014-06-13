namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_fish_transport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FishTransport_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReleaseSite = c.String(),
                        TotalFishRepresented = c.Int(),
                        ReleaseSiteComments = c.String(),
                        TransportTankUnit = c.String(),
                        TransportReleaseTemperature = c.Double(),
                        TransportReleaseTemperatureF = c.Double(),
                        TransportMortality = c.Int(),
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
                "dbo.FishTransport_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
            AddColumn("dbo.AdultWeir_Detail", "TransportTankUnit", c => c.String());
            DropColumn("dbo.AdultWeir_Header", "TransportTankUnit");
            DropColumn("dbo.AdultWeir_Header", "TransportReleaseTemperature");
            DropColumn("dbo.AdultWeir_Header", "TransportReleaseTemperatureF");
            DropColumn("dbo.AdultWeir_Header", "TransportMortality");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdultWeir_Header", "TransportMortality", c => c.Int());
            AddColumn("dbo.AdultWeir_Header", "TransportReleaseTemperatureF", c => c.Double());
            AddColumn("dbo.AdultWeir_Header", "TransportReleaseTemperature", c => c.Double());
            AddColumn("dbo.AdultWeir_Header", "TransportTankUnit", c => c.String());
            DropIndex("dbo.FishTransport_Header", new[] { "ByUserId" });
            DropIndex("dbo.FishTransport_Header", new[] { "ActivityId" });
            DropIndex("dbo.FishTransport_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.FishTransport_Detail", new[] { "ByUserId" });
            DropIndex("dbo.FishTransport_Detail", new[] { "ActivityId" });
            DropForeignKey("dbo.FishTransport_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.FishTransport_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.FishTransport_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.FishTransport_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.FishTransport_Detail", "ActivityId", "dbo.Activities");
            DropColumn("dbo.AdultWeir_Detail", "TransportTankUnit");
            DropTable("dbo.FishTransport_Header");
            DropTable("dbo.FishTransport_Detail");
        }
    }
}
