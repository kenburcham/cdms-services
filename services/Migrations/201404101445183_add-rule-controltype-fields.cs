namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrulecontroltypefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetadataProperties", "ControlType", c => c.String());
            AddColumn("dbo.DatasetFields", "Rule", c => c.String());
            DropColumn("dbo.DatasetFields", "DbTableName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DatasetFields", "DbTableName", c => c.String());
            DropColumn("dbo.DatasetFields", "Rule");
            DropColumn("dbo.MetadataProperties", "ControlType");
        }
    }
}
