namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activities_models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DatasetId = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ActivityTypeId = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityTypes", t => t.ActivityTypeId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.Datasets", t => t.DatasetId)
                .ForeignKey("dbo.Sources", t => t.SourceId)
                .Index(t => t.ActivityTypeId)
                .Index(t => t.UserId)
                .Index(t => t.LocationId)
                .Index(t => t.DatasetId)
                .Index(t => t.SourceId);
            
            CreateTable(
                "dbo.ActivityTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ActivityQAs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.QAStatus", t => t.QAStatusId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.ActivityId)
                .Index(t => t.QAStatusId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ActivityQAs", new[] { "UserId" });
            DropIndex("dbo.ActivityQAs", new[] { "QAStatusId" });
            DropIndex("dbo.ActivityQAs", new[] { "ActivityId" });
            DropIndex("dbo.Activities", new[] { "SourceId" });
            DropIndex("dbo.Activities", new[] { "DatasetId" });
            DropIndex("dbo.Activities", new[] { "LocationId" });
            DropIndex("dbo.Activities", new[] { "UserId" });
            DropIndex("dbo.Activities", new[] { "ActivityTypeId" });
            DropForeignKey("dbo.ActivityQAs", "UserId", "dbo.Users");
            DropForeignKey("dbo.ActivityQAs", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.ActivityQAs", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Activities", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.Activities", "DatasetId", "dbo.Datasets");
            DropForeignKey("dbo.Activities", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Activities", "UserId", "dbo.Users");
            DropForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes");
            DropTable("dbo.ActivityQAs");
            DropTable("dbo.ActivityTypes");
            DropTable("dbo.Activities");
        }
    }
}
