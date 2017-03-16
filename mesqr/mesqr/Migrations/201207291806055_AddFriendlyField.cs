namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendlyField : DbMigration
    {
        public override void Up()
        {
            AddColumn("Msqs", "FriendlyPosition", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("Msqs", "FriendlyPosition");
        }
    }
}
