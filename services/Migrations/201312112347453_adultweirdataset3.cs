namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adultweirdataset3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdultWeir_Header", "User_Id", "dbo.Users");
            DropIndex("dbo.AdultWeir_Header", new[] { "User_Id" });
            AddForeignKey("dbo.AdultWeir_Header", "ByUserId", "dbo.Users", "Id");
            CreateIndex("dbo.AdultWeir_Header", "ByUserId");
            DropColumn("dbo.AdultWeir_Header", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdultWeir_Header", "User_Id", c => c.Int());
            DropIndex("dbo.AdultWeir_Header", new[] { "ByUserId" });
            DropForeignKey("dbo.AdultWeir_Header", "ByUserId", "dbo.Users");
            CreateIndex("dbo.AdultWeir_Header", "User_Id");
            AddForeignKey("dbo.AdultWeir_Header", "User_Id", "dbo.Users", "Id");
        }
    }
}
