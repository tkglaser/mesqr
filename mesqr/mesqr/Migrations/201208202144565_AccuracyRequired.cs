namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AccuracyRequired : DbMigration
    {
        public override void Up()
        {
            Sql("update msqs set Accuracy = 100 where Accuracy is null");
            AlterColumn("Msqs", "Accuracy", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Msqs", "Accuracy", c => c.Double());
        }
    }
}
