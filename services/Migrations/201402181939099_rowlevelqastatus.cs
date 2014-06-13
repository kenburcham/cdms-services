namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rowlevelqastatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DatasetQAStatus1",
                c => new
                    {
                        Dataset_Id = c.Int(nullable: false),
                        QAStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Dataset_Id, t.QAStatus_Id })
                .ForeignKey("dbo.Datasets", t => t.Dataset_Id, cascadeDelete: true)
                .ForeignKey("dbo.QAStatus", t => t.QAStatus_Id, cascadeDelete: true)
                .Index(t => t.Dataset_Id)
                .Index(t => t.QAStatus_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DatasetQAStatus1", new[] { "QAStatus_Id" });
            DropIndex("dbo.DatasetQAStatus1", new[] { "Dataset_Id" });
            DropForeignKey("dbo.DatasetQAStatus1", "QAStatus_Id", "dbo.QAStatus");
            DropForeignKey("dbo.DatasetQAStatus1", "Dataset_Id", "dbo.Datasets");
            DropTable("dbo.DatasetQAStatus1");
        }
    }
}
