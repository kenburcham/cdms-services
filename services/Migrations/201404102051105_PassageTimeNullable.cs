namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PassageTimeNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdultWeir_Detail", "PassageTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdultWeir_Detail", "PassageTime", c => c.DateTime(nullable: false));
        }
    }
}
