namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creeldataset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreelSurvey_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyType = c.String(),
                        Species = c.String(),
                        Shift = c.String(),
                        Season = c.String(),
                        Surveyor = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        NumberAnglersObserved = c.Int(),
                        NumberAnglersInterviewed = c.Int(),
                        Comments = c.String(),
                        FieldSheetFile = c.String(),
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
                "dbo.CreelSurvey_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InterviewTime = c.String(),
                        FishermanName = c.String(),
                        FishermanPhone = c.String(),
                        HoursFished = c.Int(),
                        MinutesFished = c.Int(),
                        NumberFishCaught = c.Int(),
                        NumberFishKept = c.Int(),
                        TotalFishingTrips = c.Int(),
                        TotalCreelInterviews = c.Int(),
                        InterviewComments = c.String(),
                        GPSEasting = c.Decimal(precision: 18, scale: 2),
                        GPSNorthing = c.Decimal(precision: 18, scale: 2),
                        Projection = c.String(),
                        UTMZone = c.String(),
                        LocationId = c.Int(),
                        WaterBody = c.String(),
                        SectionNumber = c.String(),
                        RowId = c.Int(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .ForeignKey("dbo.QAStatus", t => t.QAStatusId)
                .Index(t => t.LocationId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId)
                .Index(t => t.QAStatusId);
            
            CreateTable(
                "dbo.CreelSurvey_Carcass",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Species = c.String(),
                        MethodCaught = c.String(),
                        Disposition = c.String(),
                        Sex = c.String(),
                        Origin = c.String(),
                        FinClips = c.String(),
                        Marks = c.String(),
                        ForkLength = c.Int(),
                        MeHPLength = c.Int(),
                        SnoutId = c.String(),
                        ScaleId = c.String(),
                        CarcassComments = c.String(),
                        ParentRowId = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CreelSurvey_Carcass", new[] { "QAStatusId" });
            DropIndex("dbo.CreelSurvey_Carcass", new[] { "ByUserId" });
            DropIndex("dbo.CreelSurvey_Carcass", new[] { "ActivityId" });
            DropIndex("dbo.CreelSurvey_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.CreelSurvey_Detail", new[] { "ByUserId" });
            DropIndex("dbo.CreelSurvey_Detail", new[] { "ActivityId" });
            DropIndex("dbo.CreelSurvey_Detail", new[] { "LocationId" });
            DropIndex("dbo.CreelSurvey_Header", new[] { "ByUserId" });
            DropIndex("dbo.CreelSurvey_Header", new[] { "ActivityId" });
            DropForeignKey("dbo.CreelSurvey_Carcass", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.CreelSurvey_Carcass", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CreelSurvey_Carcass", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CreelSurvey_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.CreelSurvey_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CreelSurvey_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CreelSurvey_Detail", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.CreelSurvey_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CreelSurvey_Header", "ActivityId", "dbo.Activities");
            DropTable("dbo.CreelSurvey_Carcass");
            DropTable("dbo.CreelSurvey_Detail");
            DropTable("dbo.CreelSurvey_Header");
        }
    }
}
