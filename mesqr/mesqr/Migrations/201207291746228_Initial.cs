namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Msqs",
                c => new
                    {
                        MsqId = c.Int(nullable: false, identity: true),
                        Message = c.String(maxLength: 256),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.MsqId);
            
        }
        
        public override void Down()
        {
            DropTable("Msqs");
        }
    }
}
