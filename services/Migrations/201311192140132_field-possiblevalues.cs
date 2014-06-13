namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fieldpossiblevalues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fields", "PossibleValues", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fields", "PossibleValues");
        }
    }
}
