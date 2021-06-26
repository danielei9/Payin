namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_3_AddMessageInServiceNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceNotifications", "Message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceNotifications", "Message");
        }
    }
}
