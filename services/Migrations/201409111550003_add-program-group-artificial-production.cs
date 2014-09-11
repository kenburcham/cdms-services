namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addprogramgroupartificialproduction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArtificialProduction_Detail", "ProgramGroup", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArtificialProduction_Detail", "ProgramGroup");
        }
    }
}
