namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedtsrappraisal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appraisal_Header", "UpdatedTSRFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appraisal_Header", "UpdatedTSRFile");
        }
    }
}
