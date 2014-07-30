namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moreappraisalfieldadditions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appraisal_Detail", "TypeOfTransaction", c => c.String());
            AddColumn("dbo.Appraisal_Detail", "PartiesInvolved", c => c.String());
            AddColumn("dbo.Appraisal_Header", "HasTimber", c => c.String());
            AddColumn("dbo.Appraisal_Header", "IsMappable", c => c.String());
            AddColumn("dbo.Appraisal_Header", "Acres", c => c.Double());
            AddColumn("dbo.Appraisal_Header", "PriorityType", c => c.String());
            AddColumn("dbo.Appraisal_Header", "LegalDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appraisal_Header", "LegalDescription");
            DropColumn("dbo.Appraisal_Header", "PriorityType");
            DropColumn("dbo.Appraisal_Header", "Acres");
            DropColumn("dbo.Appraisal_Header", "IsMappable");
            DropColumn("dbo.Appraisal_Header", "HasTimber");
            DropColumn("dbo.Appraisal_Detail", "PartiesInvolved");
            DropColumn("dbo.Appraisal_Detail", "TypeOfTransaction");
        }
    }
}
