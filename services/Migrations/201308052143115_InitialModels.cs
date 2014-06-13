namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectLocationId = c.Int(nullable: false),
                        ProjectTypeId = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId, cascadeDelete: true)
                .Index(t => t.ProjectTypeId);
            
            CreateTable(
                "dbo.ProjectTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationTypeId = c.Int(nullable: false),
                        SdeFeatureClassId = c.Int(nullable: false),
                        SdeObjectId = c.Int(nullable: false),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocationTypes", t => t.LocationTypeId, cascadeDelete: true)
                .ForeignKey("dbo.SdeFeatureClasses", t => t.SdeFeatureClassId, cascadeDelete: true)
                .Index(t => t.LocationTypeId)
                .Index(t => t.SdeFeatureClassId);
            
            CreateTable(
                "dbo.LocationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SdeFeatureClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Database = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MetadataValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MetadataPropertyId = c.Int(nullable: false),
                        RelationId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                        Values = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuditJournals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Action = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Result = c.String(),
                        Module = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(nullable: false),
                        GUID = c.String(),
                        Username = c.String(),
                        Description = c.String(),
                        LastLogin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPreferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MetadataEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MetadataProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MetadataEntityId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DataType = c.String(),
                        PossibleValues = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MetadataEntities", t => t.MetadataEntityId, cascadeDelete: true)
                .Index(t => t.MetadataEntityId);
            
            CreateTable(
                "dbo.LocationProjects",
                c => new
                    {
                        Location_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Location_Id, t.Project_Id })
                .ForeignKey("dbo.Locations", t => t.Location_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Location_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.LocationProjects", new[] { "Project_Id" });
            DropIndex("dbo.LocationProjects", new[] { "Location_Id" });
            DropIndex("dbo.MetadataProperties", new[] { "MetadataEntityId" });
            DropIndex("dbo.UserPreferences", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "OrganizationId" });
            DropIndex("dbo.Locations", new[] { "SdeFeatureClassId" });
            DropIndex("dbo.Locations", new[] { "LocationTypeId" });
            DropIndex("dbo.Projects", new[] { "ProjectTypeId" });
            DropForeignKey("dbo.LocationProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.LocationProjects", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.MetadataProperties", "MetadataEntityId", "dbo.MetadataEntities");
            DropForeignKey("dbo.UserPreferences", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Locations", "SdeFeatureClassId", "dbo.SdeFeatureClasses");
            DropForeignKey("dbo.Locations", "LocationTypeId", "dbo.LocationTypes");
            DropForeignKey("dbo.Projects", "ProjectTypeId", "dbo.ProjectTypes");
            DropTable("dbo.LocationProjects");
            DropTable("dbo.MetadataProperties");
            DropTable("dbo.MetadataEntities");
            DropTable("dbo.UserPreferences");
            DropTable("dbo.Organizations");
            DropTable("dbo.Users");
            DropTable("dbo.AuditJournals");
            DropTable("dbo.MetadataValues");
            DropTable("dbo.SdeFeatureClasses");
            DropTable("dbo.LocationTypes");
            DropTable("dbo.Locations");
            DropTable("dbo.ProjectTypes");
            DropTable("dbo.Projects");
        }
    }
}
