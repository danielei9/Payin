namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrecisionToTariffPriceAndDataValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SmartCity.Data", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("SmartCity.EnergyTariffPrices", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("SmartCity.EnergyTariffPrices", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SmartCity.Data", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
