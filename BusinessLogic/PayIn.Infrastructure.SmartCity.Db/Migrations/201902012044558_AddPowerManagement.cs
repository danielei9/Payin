namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
	using PayIn.Domain.SmartCity.Enums;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPowerManagement : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.EnergyTariffPrices", "PowerManagement", c => c.Int(nullable: false, defaultValue: (int) PowerManagementType.None));
            AddColumn("SmartCity.EnergyTariffPrices", "PowerContract", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("SmartCity.EnergyTariffPrices", "PowerContractUnit", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("SmartCity.EnergyTariffPrices", "PowerContractFactor", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 1));
            AddColumn("SmartCity.EnergyContracts", "Name", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("SmartCity.Sensors", "HasMaximeter", c => c.Boolean(nullable: false, defaultValue: false));
            DropColumn("SmartCity.EnergyContracts", "PowerMax");
            DropColumn("SmartCity.EnergyContracts", "PowerMaxUnit");
            DropColumn("SmartCity.EnergyContracts", "PowerMaxFactor");
        }
        
        public override void Down()
        {
            AddColumn("SmartCity.EnergyContracts", "PowerMaxFactor", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SmartCity.EnergyContracts", "PowerMaxUnit", c => c.String(nullable: false));
            AddColumn("SmartCity.EnergyContracts", "PowerMax", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("SmartCity.Sensors", "HasMaximeter");
            DropColumn("SmartCity.EnergyContracts", "Name");
            DropColumn("SmartCity.EnergyTariffPrices", "PowerContractFactor");
            DropColumn("SmartCity.EnergyTariffPrices", "PowerContractUnit");
            DropColumn("SmartCity.EnergyTariffPrices", "PowerContract");
            DropColumn("SmartCity.EnergyTariffPrices", "PowerManagement");
        }
    }
}
