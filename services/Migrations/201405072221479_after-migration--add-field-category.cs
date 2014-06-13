namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aftermigrationaddfieldcategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Datastores", "FieldCategoryId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Datastores", "FieldCategoryId");
        }
    }
}
