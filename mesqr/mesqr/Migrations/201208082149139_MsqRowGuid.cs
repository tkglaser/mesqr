namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class MsqRowGuid : DbMigration
    {
        public override void Up()
        {
            AddColumn("Msqs", "RowGuid", c => c.Guid(nullable: false, defaultValueSql: "newid()"));
        }
        
        public override void Down()
        {
            DropColumn("Msqs", "RowGuid");
        }
    }
}
