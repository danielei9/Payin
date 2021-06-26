namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInternalObservationToNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceNotifications", "InternalObservations", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceNotifications", "InternalObservations");
        }
    }
}
