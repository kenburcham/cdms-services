namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFinClip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Detail", "FinClip", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdultWeir_Detail", "FinClip");
        }
    }
}
