namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transportCardSaveInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportCards", "Name", c => c.String(nullable: false));
            AddColumn("dbo.TransportCards", "Entry", c => c.Int());
            AddColumn("dbo.TransportCards", "RandomId", c => c.Int());
            DropColumn("dbo.TransportCards", "DeviceName");
            DropColumn("dbo.TransportCards", "DeviceEntry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransportCards", "DeviceEntry", c => c.Int());
            AddColumn("dbo.TransportCards", "DeviceName", c => c.String(nullable: false));
            DropColumn("dbo.TransportCards", "RandomId");
            DropColumn("dbo.TransportCards", "Entry");
            DropColumn("dbo.TransportCards", "Name");
        }
    }
}
