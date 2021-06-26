namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColorToPeriod : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.EnergyTariffPeriods", "Color", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.EnergyTariffPeriods", "Color");
        }
    }
}
