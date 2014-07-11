namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appraisalfieldrequirements : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appraisal_Detail", "AppraisalType", c => c.String());
            AddColumn("dbo.Appraisal_Header", "CobellAppraisalWave", c => c.String());
            AddColumn("dbo.Appraisal_Header", "LeaseTypes", c => c.String());
            AddColumn("dbo.Appraisal_Header", "MapFiles", c => c.String());
            AddColumn("dbo.Appraisal_Header", "TSRFiles", c => c.String());
            AddColumn("dbo.Appraisal_Header", "FarmingLeaseFiles", c => c.String());
            AddColumn("dbo.Appraisal_Header", "TimberAppraisalFiles", c => c.String());
            AddColumn("dbo.Appraisal_Header", "GrazingLeaseFiles", c => c.String());
            AddColumn("dbo.Appraisal_Header", "AllotmentPhotoFiles", c => c.String());
            AddColumn("dbo.Appraisal_Header", "RegionalOfficeReviewFiles", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appraisal_Header", "RegionalOfficeReviewFiles");
            DropColumn("dbo.Appraisal_Header", "AllotmentPhotoFiles");
            DropColumn("dbo.Appraisal_Header", "GrazingLeaseFiles");
            DropColumn("dbo.Appraisal_Header", "TimberAppraisalFiles");
            DropColumn("dbo.Appraisal_Header", "FarmingLeaseFiles");
            DropColumn("dbo.Appraisal_Header", "TSRFiles");
            DropColumn("dbo.Appraisal_Header", "MapFiles");
            DropColumn("dbo.Appraisal_Header", "LeaseTypes");
            DropColumn("dbo.Appraisal_Header", "CobellAppraisalWave");
            DropColumn("dbo.Appraisal_Detail", "AppraisalType");
        }
    }
}
