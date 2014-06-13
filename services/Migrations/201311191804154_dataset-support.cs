namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datasetsupport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Datasets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        DefaultRowQAStatusId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QAStatus", t => t.DefaultRowQAStatusId, cascadeDelete: true)
                .Index(t => t.DefaultRowQAStatusId);
            
            CreateTable(
                "dbo.QAStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Dataset_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Datasets", t => t.Dataset_Id)
                .Index(t => t.Dataset_Id);
            
            CreateTable(
                "dbo.DatasetFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatasetId = c.Int(nullable: false),
                        FieldId = c.Int(nullable: false),
                        FieldRoleId = c.Int(nullable: false),
                        FieldConfigurationId = c.Int(),
                        CreateDateTime = c.DateTime(nullable: false),
                        Label = c.String(),
                        DbTableName = c.String(),
                        DbColumnName = c.String(),
                        Validation = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fields", t => t.FieldId, cascadeDelete: true)
                .ForeignKey("dbo.FieldRoles", t => t.FieldRoleId, cascadeDelete: true)
                .ForeignKey("dbo.FieldConfigurations", t => t.FieldConfigurationId)
                .Index(t => t.FieldId)
                .Index(t => t.FieldRoleId)
                .Index(t => t.FieldConfigurationId);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldCategoryId = c.Int(nullable: false),
                        TechnicalName = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Units = c.String(),
                        Validation = c.String(),
                        DataType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FieldCategories", t => t.FieldCategoryId, cascadeDelete: true)
                .Index(t => t.FieldCategoryId);
            
            CreateTable(
                "dbo.FieldCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FieldRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fields", t => t.FieldId, cascadeDelete: true)
                .ForeignKey("dbo.Sources", t => t.SourceId, cascadeDelete: true)
                .ForeignKey("dbo.Instruments", t => t.InstrumentId)
                .Index(t => t.FieldId)
                .Index(t => t.SourceId)
                .Index(t => t.InstrumentId);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Instruments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        SerialNumber = c.String(),
                        Manufacturer = c.String(),
                        OwningDepartmentId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.OwningDepartmentId, cascadeDelete: true)
                .Index(t => t.OwningDepartmentId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Instruments", new[] { "OwningDepartmentId" });
            DropIndex("dbo.FieldConfigurations", new[] { "InstrumentId" });
            DropIndex("dbo.FieldConfigurations", new[] { "SourceId" });
            DropIndex("dbo.FieldConfigurations", new[] { "FieldId" });
            DropIndex("dbo.Fields", new[] { "FieldCategoryId" });
            DropIndex("dbo.DatasetFields", new[] { "FieldConfigurationId" });
            DropIndex("dbo.DatasetFields", new[] { "FieldRoleId" });
            DropIndex("dbo.DatasetFields", new[] { "FieldId" });
            DropIndex("dbo.QAStatus", new[] { "Dataset_Id" });
            DropIndex("dbo.Datasets", new[] { "DefaultRowQAStatusId" });
            DropForeignKey("dbo.Instruments", "OwningDepartmentId", "dbo.Departments");
            DropForeignKey("dbo.FieldConfigurations", "InstrumentId", "dbo.Instruments");
            DropForeignKey("dbo.FieldConfigurations", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.FieldConfigurations", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.Fields", "FieldCategoryId", "dbo.FieldCategories");
            DropForeignKey("dbo.DatasetFields", "FieldConfigurationId", "dbo.FieldConfigurations");
            DropForeignKey("dbo.DatasetFields", "FieldRoleId", "dbo.FieldRoles");
            DropForeignKey("dbo.DatasetFields", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.QAStatus", "Dataset_Id", "dbo.Datasets");
            DropForeignKey("dbo.Datasets", "DefaultRowQAStatusId", "dbo.QAStatus");
            DropTable("dbo.Instruments");
            DropTable("dbo.Sources");
            DropTable("dbo.FieldConfigurations");
            DropTable("dbo.FieldRoles");
            DropTable("dbo.FieldCategories");
            DropTable("dbo.Fields");
            DropTable("dbo.DatasetFields");
            DropTable("dbo.QAStatus");
            DropTable("dbo.Datasets");
        }
    }
}
