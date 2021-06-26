namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_0_0_NotificationRemake : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        ReferrenceId = c.Int(nullable: false),
                        ReferrenceClass = c.String(),
                        SenderLogin = c.String(),
                        ReceiverLogin = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ServiceNotifications");
        }
    }
}
