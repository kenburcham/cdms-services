namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datasetconfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Datasets", "Config", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Datasets", "Config");
        }
    }
}
