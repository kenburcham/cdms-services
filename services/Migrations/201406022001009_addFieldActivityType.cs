namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldActivityType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterTemp_Header", "FieldActivityType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterTemp_Header", "FieldActivityType");
        }
    }
}
