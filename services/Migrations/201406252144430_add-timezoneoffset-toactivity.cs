namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtimezoneoffsettoactivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "TimezoneOffset", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "TimezoneOffset");
        }
    }
}
