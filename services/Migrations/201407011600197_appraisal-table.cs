namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appraisaltable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appraisal_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppraisalYear = c.String(),
                        AppraisalFiles = c.String(),
                        AppraisalPhotos = c.String(),
                        AppraisalComments = c.String(),
                        AppraisalStatus = c.String(),
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
                "dbo.Appraisal_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Allotment = c.String(),
                        AllotmentStatus = c.String(),
                        AllotmentName = c.String(),
                        AllotmentDescription = c.String(),
                        AllotmentComments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ByUserId)
                .Index(t => t.ActivityId)
                .Index(t => t.ByUserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Appraisal_Header", new[] { "ByUserId" });
            DropIndex("dbo.Appraisal_Header", new[] { "ActivityId" });
            DropIndex("dbo.Appraisal_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.Appraisal_Detail", new[] { "ByUserId" });
            DropIndex("dbo.Appraisal_Detail", new[] { "ActivityId" });
            DropForeignKey("dbo.Appraisal_Header", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Appraisal_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Appraisal_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.Appraisal_Detail", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.Appraisal_Detail", "ActivityId", "dbo.Activities");
            DropTable("dbo.Appraisal_Header");
            DropTable("dbo.Appraisal_Detail");
        }
    }
}
