namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
	using PayIn.Domain.SmartCity.Enums;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSensorType : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Sensors", "Type", c => c.Int(nullable: false, defaultValue: (int) SensorType.Other));
            DropColumn("SmartCity.Sensors", "Model");
        }
        
        public override void Down()
        {
            AddColumn("SmartCity.Sensors", "Model", c => c.String(nullable: false));
            DropColumn("SmartCity.Sensors", "Type");
        }
    }
}
