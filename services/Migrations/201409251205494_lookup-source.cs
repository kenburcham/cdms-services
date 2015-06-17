namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lookupsource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fields", "DataSource", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fields", "DataSource");
        }
    }
}
