namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_4_0_ZoneFromTitleToPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportTitles", "HasZone", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransportPrices", "Zone", c => c.Int());
            DropColumn("dbo.TransportTitles", "Zone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransportTitles", "Zone", c => c.Int());
            DropColumn("dbo.TransportPrices", "Zone");
            DropColumn("dbo.TransportTitles", "HasZone");
        }
    }
}
