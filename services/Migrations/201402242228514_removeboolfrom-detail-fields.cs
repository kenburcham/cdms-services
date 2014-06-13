namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeboolfromdetailfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdultWeir_Detail", "IsConventional", c => c.String());
            AlterColumn("dbo.AdultWeir_Detail", "IsRecapture", c => c.String());
            AlterColumn("dbo.AdultWeir_Detail", "IsRipe", c => c.String());
            AlterColumn("dbo.AdultWeir_Detail", "HasCodedWireTag", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdultWeir_Detail", "HasCodedWireTag", c => c.Boolean());
            AlterColumn("dbo.AdultWeir_Detail", "IsRipe", c => c.Boolean());
            AlterColumn("dbo.AdultWeir_Detail", "IsRecapture", c => c.Boolean());
            AlterColumn("dbo.AdultWeir_Detail", "IsConventional", c => c.Boolean());
        }
    }
}
