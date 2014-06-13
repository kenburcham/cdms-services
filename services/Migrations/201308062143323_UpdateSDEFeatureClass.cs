namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSDEFeatureClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SdeFeatureClasses", "Service", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SdeFeatureClasses", "Service");
        }
    }
}
