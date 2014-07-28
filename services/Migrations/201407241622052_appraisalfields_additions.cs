namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appraisalfields_additions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appraisal_Detail", "AppraisalValue", c => c.Int());
            AddColumn("dbo.Appraisal_Detail", "AppraisalValuationDate", c => c.DateTime());
            AddColumn("dbo.Appraisal_Detail", "Appraiser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appraisal_Detail", "Appraiser");
            DropColumn("dbo.Appraisal_Detail", "AppraisalValuationDate");
            DropColumn("dbo.Appraisal_Detail", "AppraisalValue");
        }
    }
}
