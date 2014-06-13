namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class instrumentaccuracycheckrename : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccuracyChecks", "InstrumentId", "dbo.Instruments");
            DropForeignKey("dbo.AccuracyChecks", "UserId", "dbo.Users");
            DropIndex("dbo.AccuracyChecks", new[] { "InstrumentId" });
            DropIndex("dbo.AccuracyChecks", new[] { "UserId" });
            CreateTable(
                "dbo.InstrumentAccuracyChecks",
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
            
            DropTable("dbo.AccuracyChecks");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.InstrumentAccuracyChecks", new[] { "UserId" });
            DropIndex("dbo.InstrumentAccuracyChecks", new[] { "InstrumentId" });
            DropForeignKey("dbo.InstrumentAccuracyChecks", "UserId", "dbo.Users");
            DropForeignKey("dbo.InstrumentAccuracyChecks", "InstrumentId", "dbo.Instruments");
            DropTable("dbo.InstrumentAccuracyChecks");
            CreateIndex("dbo.AccuracyChecks", "UserId");
            CreateIndex("dbo.AccuracyChecks", "InstrumentId");
            AddForeignKey("dbo.AccuracyChecks", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.AccuracyChecks", "InstrumentId", "dbo.Instruments", "Id");
        }
    }
}
