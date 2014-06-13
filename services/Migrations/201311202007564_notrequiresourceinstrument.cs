namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notrequiresourceinstrument : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DatasetFields", "SourceId", c => c.Int());
            AlterColumn("dbo.DatasetFields", "InstrumentId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DatasetFields", "InstrumentId", c => c.Int(nullable: false));
            AlterColumn("dbo.DatasetFields", "SourceId", c => c.Int(nullable: false));
        }
    }
}
