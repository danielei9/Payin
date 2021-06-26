namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableZoneTransportTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportTitles", "Zone", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportTitles", "Zone", c => c.Int(nullable: false));
        }
    }
}
