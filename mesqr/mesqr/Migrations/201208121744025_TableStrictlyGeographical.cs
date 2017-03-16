namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class TableStrictlyGeographical : DbMigration
    {
        public override void Up()
        {
            AddColumn("Tables", "TableRadius", c => c.Double(nullable: false));
            DropColumn("Tables", "TablePostTypeId");
            DropColumn("Tables", "TablePostRadius");
        }
        
        public override void Down()
        {
            AddColumn("Tables", "TablePostRadius", c => c.Double());
            AddColumn("Tables", "TablePostTypeId", c => c.Int(nullable: false));
            DropColumn("Tables", "TableRadius");
        }
    }
}
