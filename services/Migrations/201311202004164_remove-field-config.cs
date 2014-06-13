namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removefieldconfig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DatasetFields", "FieldConfigurationId", "dbo.FieldConfigurations");
            DropForeignKey("dbo.FieldConfigurations", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.FieldConfigurations", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.FieldConfigurations", "InstrumentId", "dbo.Instruments");
            DropIndex("dbo.DatasetFields", new[] { "FieldConfigurationId" });
            DropIndex("dbo.FieldConfigurations", new[] { "FieldId" });
            DropIndex("dbo.FieldConfigurations", new[] { "SourceId" });
            DropIndex("dbo.FieldConfigurations", new[] { "InstrumentId" });
            AddColumn("dbo.DatasetFields", "SourceId", c => c.Int(nullable: false));
            AddColumn("dbo.DatasetFields", "InstrumentId", c => c.Int(nullable: false));
            AddForeignKey("dbo.DatasetFields", "SourceId", "dbo.Sources", "Id");
            AddForeignKey("dbo.DatasetFields", "InstrumentId", "dbo.Instruments", "Id");
            CreateIndex("dbo.DatasetFields", "SourceId");
            CreateIndex("dbo.DatasetFields", "InstrumentId");
            DropColumn("dbo.DatasetFields", "FieldConfigurationId");
            DropTable("dbo.FieldConfigurations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FieldConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                        InstrumentId = c.Int(),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DatasetFields", "FieldConfigurationId", c => c.Int());
            DropIndex("dbo.DatasetFields", new[] { "InstrumentId" });
            DropIndex("dbo.DatasetFields", new[] { "SourceId" });
            DropForeignKey("dbo.DatasetFields", "InstrumentId", "dbo.Instruments");
            DropForeignKey("dbo.DatasetFields", "SourceId", "dbo.Sources");
            DropColumn("dbo.DatasetFields", "InstrumentId");
            DropColumn("dbo.DatasetFields", "SourceId");
            CreateIndex("dbo.FieldConfigurations", "InstrumentId");
            CreateIndex("dbo.FieldConfigurations", "SourceId");
            CreateIndex("dbo.FieldConfigurations", "FieldId");
            CreateIndex("dbo.DatasetFields", "FieldConfigurationId");
            AddForeignKey("dbo.FieldConfigurations", "InstrumentId", "dbo.Instruments", "Id");
            AddForeignKey("dbo.FieldConfigurations", "SourceId", "dbo.Sources", "Id");
            AddForeignKey("dbo.FieldConfigurations", "FieldId", "dbo.Fields", "Id");
            AddForeignKey("dbo.DatasetFields", "FieldConfigurationId", "dbo.FieldConfigurations", "Id");
        }
    }
}
