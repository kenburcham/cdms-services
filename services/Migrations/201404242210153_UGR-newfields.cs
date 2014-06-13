namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UGRnewfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Detail", "AgePITTag", c => c.Int());
            AddColumn("dbo.AdultWeir_Detail", "AgeCWT", c => c.Int());
            AddColumn("dbo.AdultWeir_Detail", "AgeScale", c => c.Int());
            AddColumn("dbo.AdultWeir_Detail", "AgeLength", c => c.Int());
            AddColumn("dbo.AdultWeir_Detail", "BroodProgram", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "TransportFrom", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "HatcheryType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdultWeir_Detail", "HatcheryType");
            DropColumn("dbo.AdultWeir_Detail", "TransportFrom");
            DropColumn("dbo.AdultWeir_Detail", "BroodProgram");
            DropColumn("dbo.AdultWeir_Detail", "AgeLength");
            DropColumn("dbo.AdultWeir_Detail", "AgeScale");
            DropColumn("dbo.AdultWeir_Detail", "AgeCWT");
            DropColumn("dbo.AdultWeir_Detail", "AgePITTag");
        }
    }
}
