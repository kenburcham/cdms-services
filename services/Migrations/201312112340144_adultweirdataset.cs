namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adultweirdataset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdultWeir_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sex = c.Int(nullable: false),
                        Mark = c.String(),
                        ForkLength = c.Int(nullable: false),
                        TotalLength = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        ScaleId = c.String(),
                        GeneticSampleId = c.String(),
                        Opercle = c.String(),
                        IsConventional = c.Boolean(nullable: false),
                        SnoutId = c.String(),
                        Disposition = c.String(),
                        IsRecapture = c.Boolean(nullable: false),
                        LifeStage = c.String(),
                        Origin = c.String(),
                        Age = c.String(),
                        Species = c.String(),
                        PITTag = c.String(),
                        ReleaseSiteComments = c.String(),
                        IsRipe = c.Boolean(nullable: false),
                        HasCodedWireTag = c.Boolean(nullable: false),
                        RadioTagId = c.String(),
                        Solution = c.String(),
                        SolutionDosage = c.Int(nullable: false),
                        TotalFishRepresented = c.Int(nullable: false),
                        FishComments = c.String(),
                        RowId = c.Int(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.QAStatus", t => t.QAStatusId)
                .Index(t => t.ActivityId)
                .Index(t => t.User_Id)
                .Index(t => t.QAStatusId);
            
            CreateTable(
                "dbo.AdultWeir_Header",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollectionDate = c.DateTime(nullable: false),
                        AirTemperature = c.Double(nullable: false),
                        WaterTemperature = c.Double(nullable: false),
                        TimeStart = c.DateTime(nullable: false),
                        TimeEnd = c.DateTime(nullable: false),
                        Technicians = c.String(),
                        WaterFlow = c.Int(nullable: false),
                        Comments = c.String(),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.ActivityId)
                .Index(t => t.User_Id);
            
            AddForeignKey("dbo.DatasetFields", "DatasetId", "dbo.Datasets", "Id");
            CreateIndex("dbo.DatasetFields", "DatasetId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AdultWeir_Header", new[] { "User_Id" });
            DropIndex("dbo.AdultWeir_Header", new[] { "ActivityId" });
            DropIndex("dbo.AdultWeir_Detail", new[] { "QAStatusId" });
            DropIndex("dbo.AdultWeir_Detail", new[] { "User_Id" });
            DropIndex("dbo.AdultWeir_Detail", new[] { "ActivityId" });
            DropIndex("dbo.DatasetFields", new[] { "DatasetId" });
            DropForeignKey("dbo.AdultWeir_Header", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AdultWeir_Header", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.AdultWeir_Detail", "QAStatusId", "dbo.QAStatus");
            DropForeignKey("dbo.AdultWeir_Detail", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AdultWeir_Detail", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.DatasetFields", "DatasetId", "dbo.Datasets");
            DropTable("dbo.AdultWeir_Header");
            DropTable("dbo.AdultWeir_Detail");
        }
    }
}
