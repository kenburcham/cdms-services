namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_sex_field : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdultWeir_Detail", "Sex", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdultWeir_Detail", "Sex", c => c.Int());
        }
    }
}
