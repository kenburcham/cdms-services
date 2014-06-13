namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfieldcolumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fields", "DbColumnName", c => c.String());
            AddColumn("dbo.Fields", "ControlType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fields", "ControlType");
            DropColumn("dbo.Fields", "DbColumnName");
        }
    }
}
