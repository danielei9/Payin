namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCO2FactorToDevice : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Devices", "CO2Factor", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.Devices", "CO2Factor");
        }
    }
}
