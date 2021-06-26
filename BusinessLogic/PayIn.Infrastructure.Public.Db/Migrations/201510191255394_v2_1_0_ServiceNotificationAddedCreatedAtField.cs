namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_1_0_ServiceNotificationAddedCreatedAtField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceNotifications", "CreatedAt", c => c.DateTime(nullable: false, defaultValueSql: "1900/1/1"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceNotifications", "CreatedAt");
        }
    }
}
