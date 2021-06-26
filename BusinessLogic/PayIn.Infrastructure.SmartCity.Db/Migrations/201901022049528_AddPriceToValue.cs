namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriceToValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Data", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("SmartCity.Data", "EnergyTariffPriceId", c => c.Int());
            CreateIndex("SmartCity.Data", "EnergyTariffPriceId");
            AddForeignKey("SmartCity.Data", "EnergyTariffPriceId", "SmartCity.EnergyTariffPrices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("SmartCity.Data", "EnergyTariffPriceId", "SmartCity.EnergyTariffPrices");
            DropIndex("SmartCity.Data", new[] { "EnergyTariffPriceId" });
            DropColumn("SmartCity.Data", "EnergyTariffPriceId");
            DropColumn("SmartCity.Data", "Price");
        }
    }
}
