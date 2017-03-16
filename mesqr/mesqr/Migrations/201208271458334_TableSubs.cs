namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class TableSubs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "TableSubscriptions",
                c => new
                    {
                        TableSubscriptionId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TableId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TableSubscriptionId)
                .ForeignKey("Users", t => t.UserId, cascadeDelete: false)
                .ForeignKey("Tables", t => t.TableId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.TableId);
        }
        
        public override void Down()
        {
            DropIndex("TableSubscriptions", new[] { "TableId" });
            DropIndex("TableSubscriptions", new[] { "UserId" });
            DropForeignKey("TableSubscriptions", "TableId", "Tables");
            DropForeignKey("TableSubscriptions", "UserId", "Users");
            DropTable("TableSubscriptions");
        }
    }
}
