namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Detail", "Stock", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdultWeir_Detail", "Stock");
        }
    }
}
