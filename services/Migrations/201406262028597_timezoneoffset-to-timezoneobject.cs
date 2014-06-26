namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timezoneoffsettotimezoneobject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Timezone", c => c.String());
            DropColumn("dbo.Activities", "TimezoneOffset");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "TimezoneOffset", c => c.Int());
            DropColumn("dbo.Activities", "Timezone");
        }
    }
}
