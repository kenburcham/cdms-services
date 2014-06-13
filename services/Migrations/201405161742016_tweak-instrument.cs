namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tweakinstrument : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Instruments", "PurchaseDate", c => c.DateTime());
            AlterColumn("dbo.Instruments", "EnteredService", c => c.DateTime());
            AlterColumn("dbo.Instruments", "EndedService", c => c.DateTime());
            DropColumn("dbo.Instruments", "DatastoreId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Instruments", "DatastoreId", c => c.Int(nullable: false));
            AlterColumn("dbo.Instruments", "EndedService", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Instruments", "EnteredService", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Instruments", "PurchaseDate", c => c.DateTime(nullable: false));
        }
    }
}
