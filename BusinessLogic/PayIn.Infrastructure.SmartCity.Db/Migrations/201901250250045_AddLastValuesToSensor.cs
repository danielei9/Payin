namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastValuesToSensor : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Sensors", "LastTimestamp", c => c.DateTime());
            AddColumn("SmartCity.Sensors", "LastValue", c => c.Decimal(precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.Sensors", "LastValue");
            DropColumn("SmartCity.Sensors", "LastTimestamp");
        }
    }
}
