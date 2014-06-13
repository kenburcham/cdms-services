namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGirthTrapLocationFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Detail", "Girth", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "TrapLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdultWeir_Detail", "TrapLocation");
            DropColumn("dbo.AdultWeir_Detail", "Girth");
        }
    }
}
