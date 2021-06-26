namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TarifasyMax : DbMigration
    {
        public override void Up()
        {
            AddColumn("SmartCity.Tariffs", "VoltageMax", c => c.Int());
            AddColumn("SmartCity.Tariffs", "VoltageMaxUnit", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("SmartCity.Tariffs", "VoltageMaxFactor", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 1));
            AddColumn("SmartCity.Tariffs", "PowerMax", c => c.Int());
            AddColumn("SmartCity.Tariffs", "PowerMaxUnit", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("SmartCity.Tariffs", "PowerMaxFactor", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("SmartCity.Tariffs", "PowerMaxFactor");
            DropColumn("SmartCity.Tariffs", "PowerMaxUnit");
            DropColumn("SmartCity.Tariffs", "PowerMax");
            DropColumn("SmartCity.Tariffs", "VoltageMaxFactor");
            DropColumn("SmartCity.Tariffs", "VoltageMaxUnit");
            DropColumn("SmartCity.Tariffs", "VoltageMax");
        }
    }
}
