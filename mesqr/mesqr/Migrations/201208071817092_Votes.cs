namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Votes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Votes",
                c => new
                    {
                        VoteId = c.Int(nullable: false, identity: true),
                        Up = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        MsqId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VoteId)
                .ForeignKey("Users", t => t.UserId, cascadeDelete: false)
                .ForeignKey("Msqs", t => t.MsqId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.MsqId);
            
        }
        
        public override void Down()
        {
            DropIndex("Votes", new[] { "MsqId" });
            DropIndex("Votes", new[] { "UserId" });
            DropForeignKey("Votes", "MsqId", "Msqs");
            DropForeignKey("Votes", "UserId", "Users");
            DropTable("Votes");
        }
    }
}
