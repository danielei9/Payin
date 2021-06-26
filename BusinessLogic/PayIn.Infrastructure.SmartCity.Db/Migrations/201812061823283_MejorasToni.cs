namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
	using PayIn.Domain.SmartCity.Enums;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class MejorasToni : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SmartCity.Components",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, defaultValue: ""),
                        Model = c.String(nullable: false, defaultValue: ""),
                        TimeZone = c.String(nullable: false, defaultValue: "Romance Standard Time"),
                        Type = c.Int(nullable: false, defaultValue: (int) ComponentType.EnergyMeter),
                        Longitude = c.Decimal(precision: 9, scale: 6),
                        Latitude = c.Decimal(precision: 9, scale: 6),
                    })
                .PrimaryKey(t => t.Id);

			//Sql(
			//	"INSERT SmartCity.Components" +
			//	"(Name, Model, TimeZone, Type) " +
			//	"VALUES ('Component', 'SenNet DL171', 'Romance Standard Time', 1) "
			//);

            AddColumn("SmartCity.Observations", "Latitude", c => c.Decimal(precision: 9, scale: 6));
            AddColumn("SmartCity.Observations", "Longitude", c => c.Decimal(precision: 9, scale: 6));
            AddColumn("SmartCity.Sensors", "Name", c => c.String(nullable: false, defaultValue: "Code"));
            AddColumn("SmartCity.Sensors", "Model", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("SmartCity.Sensors", "Unit", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("SmartCity.Sensors", "ComponentId", c => c.Int(nullable: false /*, defaultValue: 1*/));
            AlterColumn("SmartCity.Sensors", "Code", c => c.String(nullable: false));
            CreateIndex("SmartCity.Sensors", "ComponentId");
            AddForeignKey("SmartCity.Sensors", "ComponentId", "SmartCity.Components", "Id");
            DropColumn("SmartCity.Observations", "Location");
        }
        
        public override void Down()
        {
            AddColumn("SmartCity.Observations", "Location", c => c.String());
            DropForeignKey("SmartCity.Sensors", "ComponentId", "SmartCity.Components");
            DropIndex("SmartCity.Sensors", new[] { "ComponentId" });
            AlterColumn("SmartCity.Sensors", "Code", c => c.String());
            DropColumn("SmartCity.Sensors", "ComponentId");
            DropColumn("SmartCity.Sensors", "Unit");
            DropColumn("SmartCity.Sensors", "Model");
            DropColumn("SmartCity.Sensors", "Name");
            DropColumn("SmartCity.Observations", "Longitude");
            DropColumn("SmartCity.Observations", "Latitude");
            DropTable("SmartCity.Components");
        }
    }
}
