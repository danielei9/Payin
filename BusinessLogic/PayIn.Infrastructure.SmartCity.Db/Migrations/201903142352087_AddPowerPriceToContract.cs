namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPowerPriceToContract : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.EnergyTariffPrices", "PowerPrice", c => c.Decimal(precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.EnergyTariffPrices", "PowerPrice");
        }
    }
}
