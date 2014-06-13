namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rowstatuscolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdultWeir_Detail", "RowStatusId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdultWeir_Detail", "RowStatusId");
        }
    }
}
