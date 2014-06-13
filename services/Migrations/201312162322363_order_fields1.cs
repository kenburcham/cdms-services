namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order_fields1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DatasetFields", "OrderIndex", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DatasetFields", "OrderIndex");
        }
    }
}
