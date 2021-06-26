namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStateToEnergyTariffPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.EnergyTariffPrices", "State", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.EnergyTariffPrices", "State");
        }
    }
}
