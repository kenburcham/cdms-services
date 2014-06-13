namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adultweirdataset2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdultWeir_Detail", "Sex", c => c.Int());
            AlterColumn("dbo.AdultWeir_Detail", "ForkLength", c => c.Int());
            AlterColumn("dbo.AdultWeir_Detail", "TotalLength", c => c.Int());
            AlterColumn("dbo.AdultWeir_Detail", "Weight", c => c.Int());
            AlterColumn("dbo.AdultWeir_Detail", "IsConventional", c => c.Boolean());
            AlterColumn("dbo.AdultWeir_Detail", "IsRecapture", c => c.Boolean());
            AlterColumn("dbo.AdultWeir_Detail", "IsRipe", c => c.Boolean());
            AlterColumn("dbo.AdultWeir_Detail", "HasCodedWireTag", c => c.Boolean());
            AlterColumn("dbo.AdultWeir_Detail", "SolutionDosage", c => c.Int());
            AlterColumn("dbo.AdultWeir_Detail", "TotalFishRepresented", c => c.Int());
            AlterColumn("dbo.AdultWeir_Header", "AirTemperature", c => c.Double());
            AlterColumn("dbo.AdultWeir_Header", "WaterTemperature", c => c.Double());
            AlterColumn("dbo.AdultWeir_Header", "TimeStart", c => c.DateTime());
            AlterColumn("dbo.AdultWeir_Header", "TimeEnd", c => c.DateTime());
            AlterColumn("dbo.AdultWeir_Header", "WaterFlow", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdultWeir_Header", "WaterFlow", c => c.Int(nullable: false));
            AlterColumn("dbo.AdultWeir_Header", "TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AdultWeir_Header", "TimeStart", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AdultWeir_Header", "WaterTemperature", c => c.Double(nullable: false));
            AlterColumn("dbo.AdultWeir_Header", "AirTemperature", c => c.Double(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "TotalFishRepresented", c => c.Int(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "SolutionDosage", c => c.Int(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "HasCodedWireTag", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "IsRipe", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "IsRecapture", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "IsConventional", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "Weight", c => c.Int(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "TotalLength", c => c.Int(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "ForkLength", c => c.Int(nullable: false));
            AlterColumn("dbo.AdultWeir_Detail", "Sex", c => c.Int(nullable: false));
        }
    }
}
