namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appraisalprojecttype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appraisal_Detail", "AppraisalProjectType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appraisal_Detail", "AppraisalProjectType");
        }
    }
}
