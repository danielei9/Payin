namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGeofenceRadiousToStop : DbMigration
    {
        public override void Up()
        {
            AddColumn("Bus.Stops", "GeofenceRadious", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Bus.Stops", "GeofenceRadious");
        }
    }
}
