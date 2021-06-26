namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tarificacion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SmartCity.TariffPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PeriodId = c.Int(nullable: false),
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.TariffPeriods", t => t.PeriodId)
                .ForeignKey("SmartCity.Sensors", t => t.SensorId)
                .Index(t => t.PeriodId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "SmartCity.TariffPeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        TariffId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.Tariffs", t => t.TariffId)
                .Index(t => t.TariffId);
            
            CreateTable(
                "SmartCity.Tariffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "SmartCity.TariffSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        WeekDay = c.Int(nullable: false),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        TariffId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.Tariffs", t => t.TariffId)
                .Index(t => t.TariffId);
            
            CreateTable(
                "SmartCity.TariffTimeTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        ScheduleId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.TariffSchedules", t => t.ScheduleId)
                .ForeignKey("SmartCity.TariffPeriods", t => t.PeriodId)
                .Index(t => t.ScheduleId)
                .Index(t => t.PeriodId);
            
            AddColumn("SmartCity.Sensors", "TariffId", c => c.Int());
            CreateIndex("SmartCity.Sensors", "TariffId");
            AddForeignKey("SmartCity.Sensors", "TariffId", "SmartCity.Tariffs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("SmartCity.TariffPrices", "SensorId", "SmartCity.Sensors");
            DropForeignKey("SmartCity.TariffTimeTables", "PeriodId", "SmartCity.TariffPeriods");
            DropForeignKey("SmartCity.Sensors", "TariffId", "SmartCity.Tariffs");
            DropForeignKey("SmartCity.TariffSchedules", "TariffId", "SmartCity.Tariffs");
            DropForeignKey("SmartCity.TariffTimeTables", "ScheduleId", "SmartCity.TariffSchedules");
            DropForeignKey("SmartCity.TariffPeriods", "TariffId", "SmartCity.Tariffs");
            DropForeignKey("SmartCity.TariffPrices", "PeriodId", "SmartCity.TariffPeriods");
            DropIndex("SmartCity.TariffTimeTables", new[] { "PeriodId" });
            DropIndex("SmartCity.TariffTimeTables", new[] { "ScheduleId" });
            DropIndex("SmartCity.TariffSchedules", new[] { "TariffId" });
            DropIndex("SmartCity.TariffPeriods", new[] { "TariffId" });
            DropIndex("SmartCity.TariffPrices", new[] { "SensorId" });
            DropIndex("SmartCity.TariffPrices", new[] { "PeriodId" });
            DropIndex("SmartCity.Sensors", new[] { "TariffId" });
            DropColumn("SmartCity.Sensors", "TariffId");
            DropTable("SmartCity.TariffTimeTables");
            DropTable("SmartCity.TariffSchedules");
            DropTable("SmartCity.Tariffs");
            DropTable("SmartCity.TariffPeriods");
            DropTable("SmartCity.TariffPrices");
        }
    }
}
