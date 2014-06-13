namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddefaultqastatusfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Datasets", "DefaultActivityQAStatusId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Datasets", "DefaultActivityQAStatusId");
        }
    }
}
