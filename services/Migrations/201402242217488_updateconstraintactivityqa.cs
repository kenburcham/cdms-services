namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateconstraintactivityqa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityQAs", "ActivityId", "dbo.Activities");
            DropIndex("dbo.ActivityQAs", new[] { "ActivityId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.ActivityQAs", "ActivityId");
            AddForeignKey("dbo.ActivityQAs", "ActivityId", "dbo.Activities", "Id");
        }
    }
}
