namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adultweir_fields_tostandards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Detail", "Recapture", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "PITTagId", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "ReleaseSite", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "Ripeness", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "Tag", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "RunYear", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "OtherTagId", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "PercentSpawned", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "OtolithGenetics", c => c.String());
            AddColumn("dbo.AdultWeir_Header", "CollectionType", c => c.String());
            AddColumn("dbo.AdultWeir_Header", "TransportTankUnit", c => c.String());
            AddColumn("dbo.AdultWeir_Header", "TransportReleaseTemperature", c => c.Double());
            AddColumn("dbo.AdultWeir_Header", "TransportMortality", c => c.Int());
            AlterColumn("dbo.AdultWeir_Header", "TimeStart", c => c.String());
            AlterColumn("dbo.AdultWeir_Header", "TimeEnd", c => c.String());
            AlterColumn("dbo.AdultWeir_Header", "WaterFlow", c => c.Double());
            DropColumn("dbo.AdultWeir_Detail", "Opercle");
            DropColumn("dbo.AdultWeir_Detail", "IsConventional");
            DropColumn("dbo.AdultWeir_Detail", "IsRecapture");
            DropColumn("dbo.AdultWeir_Detail", "Age");
            DropColumn("dbo.AdultWeir_Detail", "PITTag");
            DropColumn("dbo.AdultWeir_Detail", "ReleaseSiteComments");
            DropColumn("dbo.AdultWeir_Detail", "IsRipe");
            DropColumn("dbo.AdultWeir_Detail", "HasCodedWireTag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdultWeir_Detail", "HasCodedWireTag", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "IsRipe", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "ReleaseSiteComments", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "PITTag", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "Age", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "IsRecapture", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "IsConventional", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "Opercle", c => c.String());
            AlterColumn("dbo.AdultWeir_Header", "WaterFlow", c => c.Int());
            AlterColumn("dbo.AdultWeir_Header", "TimeEnd", c => c.DateTime());
            AlterColumn("dbo.AdultWeir_Header", "TimeStart", c => c.DateTime());
            DropColumn("dbo.AdultWeir_Header", "TransportMortality");
            DropColumn("dbo.AdultWeir_Header", "TransportReleaseTemperature");
            DropColumn("dbo.AdultWeir_Header", "TransportTankUnit");
            DropColumn("dbo.AdultWeir_Header", "CollectionType");
            DropColumn("dbo.AdultWeir_Detail", "OtolithGenetics");
            DropColumn("dbo.AdultWeir_Detail", "PercentSpawned");
            DropColumn("dbo.AdultWeir_Detail", "OtherTagId");
            DropColumn("dbo.AdultWeir_Detail", "RunYear");
            DropColumn("dbo.AdultWeir_Detail", "Tag");
            DropColumn("dbo.AdultWeir_Detail", "Ripeness");
            DropColumn("dbo.AdultWeir_Detail", "ReleaseSite");
            DropColumn("dbo.AdultWeir_Detail", "PITTagId");
            DropColumn("dbo.AdultWeir_Detail", "Recapture");
        }
    }
}
