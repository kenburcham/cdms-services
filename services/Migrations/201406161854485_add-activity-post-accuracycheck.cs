namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addactivitypostaccuracycheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "PostAccuracyCheckId", c => c.Int());
            AddForeignKey("dbo.Activities", "PostAccuracyCheckId", "dbo.InstrumentAccuracyChecks", "Id");
            CreateIndex("dbo.Activities", "PostAccuracyCheckId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Activities", new[] { "PostAccuracyCheckId" });
            DropForeignKey("dbo.Activities", "PostAccuracyCheckId", "dbo.InstrumentAccuracyChecks");
            DropColumn("dbo.Activities", "PostAccuracyCheckId");
        }
    }
}
