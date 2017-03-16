namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Accuracy : DbMigration
    {
        public override void Up()
        {
            AddColumn("Msqs", "Accuracy", c => c.Double());
            AddColumn("Msqs", "Altitude", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("Msqs", "Altitude");
            DropColumn("Msqs", "Accuracy");
        }
    }
}
