namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStateToComponentAndSensor : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Components", "State", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("SmartCity.Sensors", "State", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.Sensors", "State");
            DropColumn("SmartCity.Components", "State");
        }
    }
}
