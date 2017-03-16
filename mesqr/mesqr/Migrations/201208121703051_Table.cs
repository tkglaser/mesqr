namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Tables",
                c => new
                    {
                        TableId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 500),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        TableJoinTypeId = c.Int(nullable: false),
                        TableJoinRadius = c.Double(),
                        Entered = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        RowGuid = c.Guid(nullable: false, defaultValueSql: "NEWID()"),
                    })
                .PrimaryKey(t => t.TableId);
            
            AddColumn("Msqs", "TableId", c => c.Int());
            AddForeignKey("Msqs", "TableId", "Tables", "TableId");
            CreateIndex("Msqs", "TableId");
        }
        
        public override void Down()
        {
            DropIndex("Msqs", new[] { "TableId" });
            DropForeignKey("Msqs", "TableId", "Tables");
            DropColumn("Msqs", "TableId");
            DropTable("Tables");
        }
    }
}
