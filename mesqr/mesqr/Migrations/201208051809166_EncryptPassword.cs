namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class EncryptPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "EncryptedPassword", c => c.String(nullable: false, maxLength: 250));
            DropColumn("Users", "Password");
        }
        
        public override void Down()
        {
            AddColumn("Users", "Password", c => c.String(nullable: false, maxLength: 250));
            DropColumn("Users", "EncryptedPassword");
        }
    }
}
