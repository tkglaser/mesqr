namespace mesqr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableOwner : DbMigration
    {
        public override void Up()
        {
            Sql("delete from tables");
            AddColumn("dbo.Tables", "OwnerId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Tables", "OwnerId", "dbo.Users", "UserId", cascadeDelete: false);
            CreateIndex("dbo.Tables", "OwnerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tables", new[] { "OwnerId" });
            DropForeignKey("dbo.Tables", "OwnerId", "dbo.Users");
            DropColumn("dbo.Tables", "OwnerId");
        }
    }
}
