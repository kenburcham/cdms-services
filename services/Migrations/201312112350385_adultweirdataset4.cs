namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adultweirdataset4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdultWeir_Detail", "User_Id", "dbo.Users");
            DropIndex("dbo.AdultWeir_Detail", new[] { "User_Id" });
            AddForeignKey("dbo.AdultWeir_Detail", "ByUserId", "dbo.Users", "Id");
            CreateIndex("dbo.AdultWeir_Detail", "ByUserId");
            DropColumn("dbo.AdultWeir_Detail", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdultWeir_Detail", "User_Id", c => c.Int());
            DropIndex("dbo.AdultWeir_Detail", new[] { "ByUserId" });
            DropForeignKey("dbo.AdultWeir_Detail", "ByUserId", "dbo.Users");
            CreateIndex("dbo.AdultWeir_Detail", "User_Id");
            AddForeignKey("dbo.AdultWeir_Detail", "User_Id", "dbo.Users", "Id");
        }
    }
}
