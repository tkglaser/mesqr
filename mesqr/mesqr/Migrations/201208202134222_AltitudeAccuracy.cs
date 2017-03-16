namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AltitudeAccuracy : DbMigration
    {
        public override void Up()
        {
            AddColumn("Msqs", "AltitudeAccuracy", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("Msqs", "AltitudeAccuracy");
        }
    }
}
