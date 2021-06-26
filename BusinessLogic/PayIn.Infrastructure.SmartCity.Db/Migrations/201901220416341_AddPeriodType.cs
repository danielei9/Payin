namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPeriodType : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.EnergyTariffPeriods", "Type", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.EnergyTariffPeriods", "Type");
        }
    }
}
