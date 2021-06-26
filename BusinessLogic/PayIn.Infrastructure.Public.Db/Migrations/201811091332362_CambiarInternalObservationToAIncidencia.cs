namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiarInternalObservationToAIncidencia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceIncidences", "InternalObservations", c => c.String(nullable: false));
            DropColumn("dbo.ServiceNotifications", "InternalObservations");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceNotifications", "InternalObservations", c => c.String(nullable: false, defaultValue: ""));
            DropColumn("dbo.ServiceIncidences", "InternalObservations");
        }
    }
}
