namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mmqastatusdataset : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.Locations", "LocationTypeId", "dbo.LocationTypes");
            DropForeignKey("dbo.Locations", "SdeFeatureClassId", "dbo.SdeFeatureClasses");
            DropForeignKey("dbo.Files", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Files", "UserId", "dbo.Users");
            DropForeignKey("dbo.Files", "FileTypeId", "dbo.FileTypes");
            DropForeignKey("dbo.Departments", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.UserPreferences", "UserId", "dbo.Users");
            DropForeignKey("dbo.MetadataProperties", "MetadataEntityId", "dbo.MetadataEntities");
            DropForeignKey("dbo.Datasets", "DefaultRowQAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.QAStatus", "Dataset_Id", "dbo.Datasets");
            DropForeignKey("dbo.DatasetFields", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.DatasetFields", "FieldRoleId", "dbo.FieldRoles");
            DropForeignKey("dbo.Fields", "FieldCategoryId", "dbo.FieldCategories");
            DropForeignKey("dbo.FieldConfigurations", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.FieldConfigurations", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.Instruments", "OwningDepartmentId", "dbo.Departments");
            DropIndex("dbo.Projects", new[] { "ProjectTypeId" });
            DropIndex("dbo.Locations", new[] { "LocationTypeId" });
            DropIndex("dbo.Locations", new[] { "SdeFeatureClassId" });
            DropIndex("dbo.Files", new[] { "ProjectId" });
            DropIndex("dbo.Files", new[] { "UserId" });
            DropIndex("dbo.Files", new[] { "FileTypeId" });
            DropIndex("dbo.Departments", new[] { "OrganizationId" });
            DropIndex("dbo.UserPreferences", new[] { "UserId" });
            DropIndex("dbo.MetadataProperties", new[] { "MetadataEntityId" });
            DropIndex("dbo.Datasets", new[] { "DefaultRowQAStatusId" });
            DropIndex("dbo.QAStatus", new[] { "Dataset_Id" });
            DropIndex("dbo.DatasetFields", new[] { "FieldId" });
            DropIndex("dbo.DatasetFields", new[] { "FieldRoleId" });
            DropIndex("dbo.Fields", new[] { "FieldCategoryId" });
            DropIndex("dbo.FieldConfigurations", new[] { "FieldId" });
            DropIndex("dbo.FieldConfigurations", new[] { "SourceId" });
            DropIndex("dbo.Instruments", new[] { "OwningDepartmentId" });
            CreateTable(
                "dbo.DatasetQAStatus",
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
            
            AddForeignKey("dbo.Projects", "ProjectTypeId", "dbo.ProjectTypes", "Id");
            AddForeignKey("dbo.Locations", "LocationTypeId", "dbo.LocationTypes", "Id");
            AddForeignKey("dbo.Locations", "SdeFeatureClassId", "dbo.SdeFeatureClasses", "Id");
            AddForeignKey("dbo.Files", "ProjectId", "dbo.Projects", "Id");
            AddForeignKey("dbo.Files", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Files", "FileTypeId", "dbo.FileTypes", "Id");
            AddForeignKey("dbo.Departments", "OrganizationId", "dbo.Organizations", "Id");
            AddForeignKey("dbo.UserPreferences", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.MetadataProperties", "MetadataEntityId", "dbo.MetadataEntities", "Id");
            AddForeignKey("dbo.Datasets", "DefaultRowQAStatusId", "dbo.QAStatus", "Id");
            AddForeignKey("dbo.DatasetFields", "FieldId", "dbo.Fields", "Id");
            AddForeignKey("dbo.DatasetFields", "FieldRoleId", "dbo.FieldRoles", "Id");
            AddForeignKey("dbo.Fields", "FieldCategoryId", "dbo.FieldCategories", "Id");
            AddForeignKey("dbo.FieldConfigurations", "FieldId", "dbo.Fields", "Id");
            AddForeignKey("dbo.FieldConfigurations", "SourceId", "dbo.Sources", "Id");
            AddForeignKey("dbo.Instruments", "OwningDepartmentId", "dbo.Departments", "Id");
            CreateIndex("dbo.Projects", "ProjectTypeId");
            CreateIndex("dbo.Locations", "LocationTypeId");
            CreateIndex("dbo.Locations", "SdeFeatureClassId");
            CreateIndex("dbo.Files", "ProjectId");
            CreateIndex("dbo.Files", "UserId");
            CreateIndex("dbo.Files", "FileTypeId");
            CreateIndex("dbo.Departments", "OrganizationId");
            CreateIndex("dbo.UserPreferences", "UserId");
            CreateIndex("dbo.MetadataProperties", "MetadataEntityId");
            CreateIndex("dbo.Datasets", "DefaultRowQAStatusId");
            CreateIndex("dbo.DatasetFields", "FieldId");
            CreateIndex("dbo.DatasetFields", "FieldRoleId");
            CreateIndex("dbo.Fields", "FieldCategoryId");
            CreateIndex("dbo.FieldConfigurations", "FieldId");
            CreateIndex("dbo.FieldConfigurations", "SourceId");
            CreateIndex("dbo.Instruments", "OwningDepartmentId");
            DropColumn("dbo.QAStatus", "Dataset_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QAStatus", "Dataset_Id", c => c.Int());
            DropIndex("dbo.DatasetQAStatus", new[] { "QAStatus_Id" });
            DropIndex("dbo.DatasetQAStatus", new[] { "Dataset_Id" });
            DropIndex("dbo.Instruments", new[] { "OwningDepartmentId" });
            DropIndex("dbo.FieldConfigurations", new[] { "SourceId" });
            DropIndex("dbo.FieldConfigurations", new[] { "FieldId" });
            DropIndex("dbo.Fields", new[] { "FieldCategoryId" });
            DropIndex("dbo.DatasetFields", new[] { "FieldRoleId" });
            DropIndex("dbo.DatasetFields", new[] { "FieldId" });
            DropIndex("dbo.Datasets", new[] { "DefaultRowQAStatusId" });
            DropIndex("dbo.MetadataProperties", new[] { "MetadataEntityId" });
            DropIndex("dbo.UserPreferences", new[] { "UserId" });
            DropIndex("dbo.Departments", new[] { "OrganizationId" });
            DropIndex("dbo.Files", new[] { "FileTypeId" });
            DropIndex("dbo.Files", new[] { "UserId" });
            DropIndex("dbo.Files", new[] { "ProjectId" });
            DropIndex("dbo.Locations", new[] { "SdeFeatureClassId" });
            DropIndex("dbo.Locations", new[] { "LocationTypeId" });
            DropIndex("dbo.Projects", new[] { "ProjectTypeId" });
            DropForeignKey("dbo.DatasetQAStatus", "QAStatus_Id", "dbo.QAStatus");
            DropForeignKey("dbo.DatasetQAStatus", "Dataset_Id", "dbo.Datasets");
            DropForeignKey("dbo.Instruments", "OwningDepartmentId", "dbo.Departments");
            DropForeignKey("dbo.FieldConfigurations", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.FieldConfigurations", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.Fields", "FieldCategoryId", "dbo.FieldCategories");
            DropForeignKey("dbo.DatasetFields", "FieldRoleId", "dbo.FieldRoles");
            DropForeignKey("dbo.DatasetFields", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.Datasets", "DefaultRowQAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.MetadataProperties", "MetadataEntityId", "dbo.MetadataEntities");
            DropForeignKey("dbo.UserPreferences", "UserId", "dbo.Users");
            DropForeignKey("dbo.Departments", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Files", "FileTypeId", "dbo.FileTypes");
            DropForeignKey("dbo.Files", "UserId", "dbo.Users");
            DropForeignKey("dbo.Files", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Locations", "SdeFeatureClassId", "dbo.SdeFeatureClasses");
            DropForeignKey("dbo.Locations", "LocationTypeId", "dbo.LocationTypes");
            DropForeignKey("dbo.Projects", "ProjectTypeId", "dbo.ProjectTypes");
            DropTable("dbo.DatasetQAStatus");
            CreateIndex("dbo.Instruments", "OwningDepartmentId");
            CreateIndex("dbo.FieldConfigurations", "SourceId");
            CreateIndex("dbo.FieldConfigurations", "FieldId");
            CreateIndex("dbo.Fields", "FieldCategoryId");
            CreateIndex("dbo.DatasetFields", "FieldRoleId");
            CreateIndex("dbo.DatasetFields", "FieldId");
            CreateIndex("dbo.QAStatus", "Dataset_Id");
            CreateIndex("dbo.Datasets", "DefaultRowQAStatusId");
            CreateIndex("dbo.MetadataProperties", "MetadataEntityId");
            CreateIndex("dbo.UserPreferences", "UserId");
            CreateIndex("dbo.Departments", "OrganizationId");
            CreateIndex("dbo.Files", "FileTypeId");
            CreateIndex("dbo.Files", "UserId");
            CreateIndex("dbo.Files", "ProjectId");
            CreateIndex("dbo.Locations", "SdeFeatureClassId");
            CreateIndex("dbo.Locations", "LocationTypeId");
            CreateIndex("dbo.Projects", "ProjectTypeId");
            AddForeignKey("dbo.Instruments", "OwningDepartmentId", "dbo.Departments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FieldConfigurations", "SourceId", "dbo.Sources", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FieldConfigurations", "FieldId", "dbo.Fields", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Fields", "FieldCategoryId", "dbo.FieldCategories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DatasetFields", "FieldRoleId", "dbo.FieldRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DatasetFields", "FieldId", "dbo.Fields", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QAStatus", "Dataset_Id", "dbo.Datasets", "Id");
            AddForeignKey("dbo.Datasets", "DefaultRowQAStatusId", "dbo.QAStatus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MetadataProperties", "MetadataEntityId", "dbo.MetadataEntities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserPreferences", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Departments", "OrganizationId", "dbo.Organizations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Files", "FileTypeId", "dbo.FileTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Files", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Files", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Locations", "SdeFeatureClassId", "dbo.SdeFeatureClasses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Locations", "LocationTypeId", "dbo.LocationTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Projects", "ProjectTypeId", "dbo.ProjectTypes", "Id", cascadeDelete: true);
        }
    }
}
