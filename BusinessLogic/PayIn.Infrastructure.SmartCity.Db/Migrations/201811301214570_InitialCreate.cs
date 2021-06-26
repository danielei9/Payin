namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SmartCity.Alarms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Sender = c.String(nullable: false),
                        AlertId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.Alerts", t => t.AlertId)
                .Index(t => t.AlertId);
            
            CreateTable(
                "SmartCity.Alerts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "SmartCity.Observations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Timestamp = c.DateTime(nullable: false),
                        Location = c.String(),
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.Sensors", t => t.SensorId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "SmartCity.Sensors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProviderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.Providers", t => t.ProviderId)
                .Index(t => t.ProviderId);
            
            CreateTable(
                "SmartCity.Providers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SmartCity.Sensors", "ProviderId", "SmartCity.Providers");
            DropForeignKey("SmartCity.Observations", "SensorId", "SmartCity.Sensors");
            DropForeignKey("SmartCity.Alarms", "AlertId", "SmartCity.Alerts");
            DropIndex("SmartCity.Sensors", new[] { "ProviderId" });
            DropIndex("SmartCity.Observations", new[] { "SensorId" });
            DropIndex("SmartCity.Alarms", new[] { "AlertId" });
            DropTable("SmartCity.Providers");
            DropTable("SmartCity.Sensors");
            DropTable("SmartCity.Observations");
            DropTable("SmartCity.Alerts");
            DropTable("SmartCity.Alarms");
        }
    }
}
