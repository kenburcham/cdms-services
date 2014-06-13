namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activityinstrument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "InstrumentId", c => c.Int());
            AddColumn("dbo.Activities", "AccuracyCheckId", c => c.Int());
            AddForeignKey("dbo.Activities", "InstrumentId", "dbo.Instruments", "Id");
            AddForeignKey("dbo.Activities", "AccuracyCheckId", "dbo.InstrumentAccuracyChecks", "Id");
            CreateIndex("dbo.Activities", "InstrumentId");
            CreateIndex("dbo.Activities", "AccuracyCheckId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Activities", new[] { "AccuracyCheckId" });
            DropIndex("dbo.Activities", new[] { "InstrumentId" });
            DropForeignKey("dbo.Activities", "AccuracyCheckId", "dbo.InstrumentAccuracyChecks");
            DropForeignKey("dbo.Activities", "InstrumentId", "dbo.Instruments");
            DropColumn("dbo.Activities", "AccuracyCheckId");
            DropColumn("dbo.Activities", "InstrumentId");
        }
    }
}
