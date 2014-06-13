namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldrule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fields", "Rule", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fields", "Rule");
        }
    }
}
