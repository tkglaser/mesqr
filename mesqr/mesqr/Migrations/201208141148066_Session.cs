namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Session : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Sessions",
                c => new
                    {
                        SessionId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Start = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        Expiry = c.DateTime(nullable: false),
                        Key = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.SessionId)
                .ForeignKey("Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);            
        }
        
        public override void Down()
        {
            DropIndex("Sessions", new[] { "UserId" });
            DropForeignKey("Sessions", "UserId", "Users");
            DropTable("Sessions");
        }
    }
}
