namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appraisalfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appraisal_Detail", "AppraisalLogNumber", c => c.String());
            AddColumn("dbo.Appraisal_Header", "LastAppraisalRequestDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appraisal_Header", "LastAppraisalRequestDate");
            DropColumn("dbo.Appraisal_Detail", "AppraisalLogNumber");
        }
    }
}
