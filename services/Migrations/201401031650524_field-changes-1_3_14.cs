namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fieldchanges1_3_14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Detail", "OtolithNumber", c => c.String());
            AddColumn("dbo.AdultWeir_Header", "EventDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.AdultWeir_Header", "CollectionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdultWeir_Header", "CollectionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.AdultWeir_Header", "EventDate");
            DropColumn("dbo.AdultWeir_Detail", "OtolithNumber");
        }
    }
}
