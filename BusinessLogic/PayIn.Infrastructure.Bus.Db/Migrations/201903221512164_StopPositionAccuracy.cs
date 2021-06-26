namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StopPositionAccuracy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Bus.Stops", "Longitude", c => c.Decimal(precision: 9, scale: 6));
            AlterColumn("Bus.Stops", "Latitude", c => c.Decimal(precision: 9, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("Bus.Stops", "Latitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("Bus.Stops", "Longitude", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
