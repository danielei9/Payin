namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapUrlforEventTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "MapUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "MapUrl");
        }
    }
}
