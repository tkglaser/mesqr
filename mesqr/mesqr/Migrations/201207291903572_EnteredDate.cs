namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class EnteredDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("Msqs", "Entered", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            DropColumn("Msqs", "Entered");
        }
    }
}
