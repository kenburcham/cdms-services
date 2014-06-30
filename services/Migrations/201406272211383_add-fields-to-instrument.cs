namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldstoinstrument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Instruments", "Model", c => c.String());
            AddColumn("dbo.Instruments", "PurchasingProgramProjectId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Instruments", "PurchasingProgramProjectId");
            DropColumn("dbo.Instruments", "Model");
        }
    }
}
