namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class filetypeextensions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileTypes", "Extensions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileTypes", "Extensions");
        }
    }
}
