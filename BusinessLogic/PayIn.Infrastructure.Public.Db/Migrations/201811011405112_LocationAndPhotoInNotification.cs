namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationAndPhotoInNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceNotifications", "PhotoUrl", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("dbo.ServiceNotifications", "Longitude", c => c.Decimal(precision: 9, scale: 6));
            AddColumn("dbo.ServiceNotifications", "Latitude", c => c.Decimal(precision: 9, scale: 6));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceNotifications", "Latitude");
            DropColumn("dbo.ServiceNotifications", "Longitude");
            DropColumn("dbo.ServiceNotifications", "PhotoUrl");
        }
    }
}
