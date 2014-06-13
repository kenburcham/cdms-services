namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Datastorefordataset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Datastores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TablePrefix = c.String(),
                        DatastoreDatasetId = c.String(),
                        OwnerUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Datasets", "DatastoreId", c => c.Int());
            AddForeignKey("dbo.Datasets", "DatastoreId", "dbo.Datastores", "Id");
            CreateIndex("dbo.Datasets", "DatastoreId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Datasets", new[] { "DatastoreId" });
            DropForeignKey("dbo.Datasets", "DatastoreId", "dbo.Datastores");
            DropColumn("dbo.Datasets", "DatastoreId");
            DropTable("dbo.Datastores");
        }
    }
}
