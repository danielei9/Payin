namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAcumulativeToSensor : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Sensors", "IsAcumulative", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.Sensors", "IsAcumulative");
        }
    }
}
