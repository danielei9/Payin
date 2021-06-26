namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContractAndAmpliation : DbMigration
    {
        public override void Up()
		{
			DropForeignKey("SmartCity.Sensors", "TariffId", "SmartCity.Tariffs");
			DropForeignKey("SmartCity.TariffPrices", "SensorId", "SmartCity.Sensors");
			RenameTable(name: "SmartCity.TariffPeriods", newName: "EnergyTariffPeriods");
            RenameTable(name: "SmartCity.Tariffs", newName: "EnergyTariffs");
            RenameTable(name: "SmartCity.TariffSchedules", newName: "EnergyTariffSchedules");
            RenameTable(name: "SmartCity.TariffTimeTables", newName: "EnergyTariffTimeTables");
            RenameTable(name: "SmartCity.TariffPrices", newName: "EnergyTariffPrices");
            DropIndex("SmartCity.Sensors", new[] { "TariffId" });
            DropIndex("SmartCity.EnergyTariffPrices", new[] { "SensorId" });
            CreateTable(
                "SmartCity.EnergyContracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company = c.String(nullable: false),
                        Reference = c.String(nullable: false),
                        PowerMax = c.Decimal(precision: 18, scale: 2),
                        PowerMaxUnit = c.String(nullable: false),
                        PowerMaxFactor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TariffId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.EnergyTariffs", t => t.TariffId)
                .Index(t => t.TariffId);
            
            AddColumn("SmartCity.Sensors", "EnergyContractId", c => c.Int());
            AddColumn("SmartCity.EnergyTariffPrices", "ContractId", c => c.Int(nullable: false));
            AlterColumn("SmartCity.EnergyTariffs", "VoltageMax", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("SmartCity.EnergyTariffs", "PowerMax", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("SmartCity.Sensors", "EnergyContractId");
            CreateIndex("SmartCity.EnergyTariffPrices", "ContractId");
            AddForeignKey("SmartCity.EnergyTariffPrices", "ContractId", "SmartCity.EnergyContracts", "Id");
            AddForeignKey("SmartCity.Sensors", "EnergyContractId", "SmartCity.EnergyContracts", "Id");
            DropColumn("SmartCity.Sensors", "TariffId");
            DropColumn("SmartCity.EnergyTariffPrices", "SensorId");
        }
        
        public override void Down()
        {
            AddColumn("SmartCity.EnergyTariffPrices", "SensorId", c => c.Int(nullable: false));
            AddColumn("SmartCity.Sensors", "TariffId", c => c.Int());
            DropForeignKey("SmartCity.Sensors", "EnergyContractId", "SmartCity.EnergyContracts");
            DropForeignKey("SmartCity.EnergyTariffPrices", "ContractId", "SmartCity.EnergyContracts");
            DropForeignKey("SmartCity.EnergyContracts", "TariffId", "SmartCity.EnergyTariffs");
            DropIndex("SmartCity.EnergyTariffPrices", new[] { "ContractId" });
            DropIndex("SmartCity.EnergyContracts", new[] { "TariffId" });
            DropIndex("SmartCity.Sensors", new[] { "EnergyContractId" });
            AlterColumn("SmartCity.EnergyTariffs", "PowerMax", c => c.Int());
            AlterColumn("SmartCity.EnergyTariffs", "VoltageMax", c => c.Int());
            DropColumn("SmartCity.EnergyTariffPrices", "ContractId");
            DropColumn("SmartCity.Sensors", "EnergyContractId");
            DropTable("SmartCity.EnergyContracts");
            CreateIndex("SmartCity.EnergyTariffPrices", "SensorId");
            CreateIndex("SmartCity.Sensors", "TariffId");
            AddForeignKey("SmartCity.TariffPrices", "SensorId", "SmartCity.Sensors", "Id");
            AddForeignKey("SmartCity.Sensors", "TariffId", "SmartCity.Tariffs", "Id");
            RenameTable(name: "SmartCity.EnergyTariffPrices", newName: "TariffPrices");
            RenameTable(name: "SmartCity.EnergyTariffTimeTables", newName: "TariffTimeTables");
            RenameTable(name: "SmartCity.EnergyTariffSchedules", newName: "TariffSchedules");
            RenameTable(name: "SmartCity.EnergyTariffs", newName: "Tariffs");
            RenameTable(name: "SmartCity.EnergyTariffPeriods", newName: "TariffPeriods");
        }
    }
}
