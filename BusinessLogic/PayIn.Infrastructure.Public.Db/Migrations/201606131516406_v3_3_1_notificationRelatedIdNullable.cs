namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_1_notificationRelatedIdNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceNotifications", "ReferenceId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceNotifications", "ReferenceId", c => c.Int(nullable: false));
        }
    }
}
