namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_2_AddStateInTransportCard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportCards", "DeviceEntry", c => c.Int());
            AddColumn("dbo.TransportCards", "State", c => c.Int(nullable: false, defaultValue: 1));
            DropColumn("dbo.TransportCards", "Entry");
            DropColumn("dbo.TransportCards", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransportCards", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransportCards", "Entry", c => c.Int());
            DropColumn("dbo.TransportCards", "State");
            DropColumn("dbo.TransportCards", "DeviceEntry");
        }
    }
}
