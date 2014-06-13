namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class controltypefield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DatasetFields", "ControlType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DatasetFields", "ControlType");
        }
    }
}
