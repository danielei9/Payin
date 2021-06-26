namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessControl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessControlEntrances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AccessControlId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessControls", t => t.AccessControlId)
                .Index(t => t.AccessControlId);
            
            CreateTable(
                "dbo.AccessControls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Schedule = c.String(nullable: false),
                        MapUrl = c.String(),
                        CurrentCapacity = c.Int(nullable: false),
                        MaxCapacity = c.Int(nullable: false),
                        PaymentConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentConcessions", t => t.PaymentConcessionId)
                .Index(t => t.PaymentConcessionId);
            
            CreateTable(
                "dbo.AccessControlEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntryDateTime = c.DateTime(nullable: false),
                        PeopleEntry = c.Int(nullable: false),
                        CapacityAfterEntrance = c.Int(nullable: false),
                        AccessControlEntranceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessControlEntrances", t => t.AccessControlEntranceId)
                .Index(t => t.AccessControlEntranceId);
            
            CreateTable(
                "dbo.AccessControlSentiloSensors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Temperature = c.String(nullable: false),
                        Humidity = c.String(nullable: false),
                        WindSpeed = c.String(nullable: false),
                        WindDirection = c.String(nullable: false),
                        BarometricPressure = c.String(nullable: false),
                        UVIndex = c.String(nullable: false),
                        SolarRadiation = c.String(nullable: false),
                        AccessControlId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessControls", t => t.AccessControlId)
                .Index(t => t.AccessControlId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccessControlSentiloSensors", "AccessControlId", "dbo.AccessControls");
            DropForeignKey("dbo.AccessControlEntries", "AccessControlEntranceId", "dbo.AccessControlEntrances");
            DropForeignKey("dbo.AccessControls", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.AccessControlEntrances", "AccessControlId", "dbo.AccessControls");
            DropIndex("dbo.AccessControlSentiloSensors", new[] { "AccessControlId" });
            DropIndex("dbo.AccessControlEntries", new[] { "AccessControlEntranceId" });
            DropIndex("dbo.AccessControls", new[] { "PaymentConcessionId" });
            DropIndex("dbo.AccessControlEntrances", new[] { "AccessControlId" });
            DropTable("dbo.AccessControlSentiloSensors");
            DropTable("dbo.AccessControlEntries");
            DropTable("dbo.AccessControls");
            DropTable("dbo.AccessControlEntrances");
        }
    }
}
