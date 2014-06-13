namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPassageFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Detail", "PassageLocation", c => c.String());
            AddColumn("dbo.AdultWeir_Detail", "PassageTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdultWeir_Detail", "PassageTime");
            DropColumn("dbo.AdultWeir_Detail", "PassageLocation");
        }
    }
}
