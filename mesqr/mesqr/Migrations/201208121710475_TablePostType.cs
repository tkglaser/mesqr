namespace mesqr.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class TablePostType : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Tables", name: "TableJoinTypeId", newName: "TablePostTypeId");
            RenameColumn(table: "Tables", name: "TableJoinRadius", newName: "TablePostRadius");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Tables", name: "TablePostTypeId", newName: "TableJoinTypeId");
            RenameColumn(table: "Tables", name: "TablePostRadius", newName: "TableJoinRadius");
        }
    }
}
