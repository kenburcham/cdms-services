namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class instrumentaccuracycheck : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Instruments", "InstrumentType_Id", "dbo.InstrumentTypes");
            DropIndex("dbo.Instruments", new[] { "InstrumentType_Id" });
            CreateTable(
                "dbo.AccuracyChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstrumentId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        CheckDate = c.DateTime(nullable: false),
                        CheckMethod = c.Int(nullable: false),
                        Bath1Grade = c.String(),
                        Bath2Grade = c.String(),
                        Comments = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instruments", t => t.InstrumentId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.InstrumentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProjectInstruments",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Instrument_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Instrument_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Instruments", t => t.Instrument_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Instrument_Id);
            
            AddColumn("dbo.Instruments", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Instruments", "StatusId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Instruments", "UserId", "dbo.Users", "Id");
            CreateIndex("dbo.Instruments", "UserId");
            DropColumn("dbo.Instruments", "InstrumentTypeId");
            DropColumn("dbo.Instruments", "DataQualityLevel");
            DropColumn("dbo.Instruments", "AccuracyCheckMethod");
            DropColumn("dbo.Instruments", "InstrumentType_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Instruments", "InstrumentType_Id", c => c.Int());
            AddColumn("dbo.Instruments", "AccuracyCheckMethod", c => c.String());
            AddColumn("dbo.Instruments", "DataQualityLevel", c => c.String());
            AddColumn("dbo.Instruments", "InstrumentTypeId", c => c.String());
            DropIndex("dbo.ProjectInstruments", new[] { "Instrument_Id" });
            DropIndex("dbo.ProjectInstruments", new[] { "Project_Id" });
            DropIndex("dbo.AccuracyChecks", new[] { "UserId" });
            DropIndex("dbo.AccuracyChecks", new[] { "InstrumentId" });
            DropIndex("dbo.Instruments", new[] { "UserId" });
            DropForeignKey("dbo.ProjectInstruments", "Instrument_Id", "dbo.Instruments");
            DropForeignKey("dbo.ProjectInstruments", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.AccuracyChecks", "UserId", "dbo.Users");
            DropForeignKey("dbo.AccuracyChecks", "InstrumentId", "dbo.Instruments");
            DropForeignKey("dbo.Instruments", "UserId", "dbo.Users");
            DropColumn("dbo.Instruments", "StatusId");
            DropColumn("dbo.Instruments", "UserId");
            DropTable("dbo.ProjectInstruments");
            DropTable("dbo.AccuracyChecks");
            CreateIndex("dbo.Instruments", "InstrumentType_Id");
            AddForeignKey("dbo.Instruments", "InstrumentType_Id", "dbo.InstrumentTypes", "Id");
        }
    }
}
