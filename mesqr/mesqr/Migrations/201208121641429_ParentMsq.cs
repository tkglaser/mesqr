namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ParentMsq : DbMigration
    {
        public override void Up()
        {
            AddColumn("Msqs", "ParentMsqId", c => c.Int());
            AddForeignKey("Msqs", "ParentMsqId", "Msqs", "MsqId");
            CreateIndex("Msqs", "ParentMsqId");
        }
        
        public override void Down()
        {
            DropIndex("Msqs", new[] { "ParentMsqId" });
            DropForeignKey("Msqs", "ParentMsqId", "Msqs");
            DropColumn("Msqs", "ParentMsqId");
        }
    }
}
