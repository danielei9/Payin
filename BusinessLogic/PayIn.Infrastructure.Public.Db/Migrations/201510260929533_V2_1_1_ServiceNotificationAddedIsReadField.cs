namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2_1_1_ServiceNotificationAddedIsReadField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceNotifications", "IsRead", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceNotifications", "IsRead");
        }
    }
}
