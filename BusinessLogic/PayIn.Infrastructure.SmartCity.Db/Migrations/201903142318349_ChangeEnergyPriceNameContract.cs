namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEnergyPriceNameContract : DbMigration
    {
        public override void Up()
        {
			AlterColumn("SmartCity.EnergyTariffPrices", "Price", c => c.Decimal(precision: 18, scale: 6));
			RenameColumn("SmartCity.EnergyTariffPrices", "Price", "EnergyPrice");
			AlterColumn("SmartCity.Data", "Price", c => c.Decimal(precision: 18, scale: 6));
        }
        
        public override void Down()
        {
			AlterColumn("SmartCity.EnergyTariffPrices", "EnergyPrice", c => c.Decimal(nullable: false, precision: 18, scale: 6));
			RenameColumn("SmartCity.EnergyTariffPrices", "EnergyPrice", "Price");
			AlterColumn("SmartCity.Data", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
    }
}
