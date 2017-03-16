namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Msqs", name: "User_UserId", newName: "UserId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Msqs", name: "UserId", newName: "User_UserId");
        }
    }
}
