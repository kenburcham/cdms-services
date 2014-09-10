namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class artificialproduction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArtificialProduction_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                "dbo.ArtificialProduction_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RunYear = c.String(),
                        Species = c.String(),
                        Origin = c.String(),
                        Sex = c.String(),
                        Disposition = c.String(),
                        TotalFishRepresented = c.Int(),
                        LifeStage = c.String(),
                        FinClip = c.String(),
                        Tag = c.String(),
                        NumberEggsTaken = c.Int(),
                        ReleaseSite = c.String(),
                        FishComments = c.String(),
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
            DropIndex("dbo.ArtificialProduction_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.ArtificialProduction_Detail", new[] { "ByUserId" });
            DropIndex("dbo.ArtificialProduction_Detail", new[] { "ActivityId" });
            DropIndex("dbo.ArtificialProduction_Header", new[] { "ByUserId" });
            DropIndex("dbo.ArtificialProduction_Header", new[] { "ActivityId" });
            DropForeignKey("dbo.ArtificialProduction_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.ArtificialProduction_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ArtificialProduction_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.ArtificialProduction_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.ArtificialProduction_Header", "ActivityId", "dbo.Activities");
            DropTable("dbo.ArtificialProduction_Detail");
            DropTable("dbo.ArtificialProduction_Header");
        }
    }
}
