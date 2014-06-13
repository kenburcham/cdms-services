namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moveeventdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "ActivityDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.AdultWeir_Header", "EventDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdultWeir_Header", "EventDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Activities", "ActivityDate");
        }
    }
}
