namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Users : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 150),
                        Password = c.String(nullable: false, maxLength: 250),
                        EMail = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.UserId);

            Sql("delete from Msqs");

            AddColumn("Msqs", "User_UserId", c => c.Int(nullable: false));
            AddForeignKey("Msqs", "User_UserId", "Users", "UserId", cascadeDelete: false);
            CreateIndex("Msqs", "User_UserId");
        }
        
        public override void Down()
        {
            DropIndex("Msqs", new[] { "User_UserId" });
            DropForeignKey("Msqs", "User_UserId", "Users");
            DropColumn("Msqs", "User_UserId");
            DropTable("Users");
        }
    }
}
