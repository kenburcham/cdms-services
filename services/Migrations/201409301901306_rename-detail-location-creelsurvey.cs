namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedetaillocationcreelsurvey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CreelSurvey_Detail", "LocationId", "dbo.Locations");
            DropIndex("dbo.CreelSurvey_Detail", new[] { "LocationId" });
            AddColumn("dbo.CreelSurvey_Detail", "DetailLocationId", c => c.Int());
            AddForeignKey("dbo.CreelSurvey_Detail", "DetailLocationId", "dbo.Locations", "Id");
            CreateIndex("dbo.CreelSurvey_Detail", "DetailLocationId");
            DropColumn("dbo.CreelSurvey_Detail", "LocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreelSurvey_Detail", "LocationId", c => c.Int());
            DropIndex("dbo.CreelSurvey_Detail", new[] { "DetailLocationId" });
            DropForeignKey("dbo.CreelSurvey_Detail", "DetailLocationId", "dbo.Locations");
            DropColumn("dbo.CreelSurvey_Detail", "DetailLocationId");
            CreateIndex("dbo.CreelSurvey_Detail", "LocationId");
            AddForeignKey("dbo.CreelSurvey_Detail", "LocationId", "dbo.Locations", "Id");
        }
    }
}
