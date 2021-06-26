namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialUpdated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        TaxName = c.String(nullable: false),
                        TaxNumber = c.String(nullable: false),
                        TaxAddress = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMediaId = c.Int(nullable: false),
                        TicketId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentMedias", t => t.PaymentMediaId)
                .ForeignKey("dbo.ServiceSuppliers", t => t.SupplierId)
                .ForeignKey("dbo.Tickets", t => t.TicketId)
                .Index(t => t.PaymentMediaId)
                .Index(t => t.TicketId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.PaymentMedias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        NumberHash = c.String(nullable: false),
                        VisualOrder = c.Int(nullable: false),
                        VisualOrderFavorite = c.Int(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceSuppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        TaxName = c.String(nullable: false),
                        TaxNumber = c.String(nullable: false),
                        TaxAddress = c.String(nullable: false),
                        PaymentTest = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceConcessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceSuppliers", t => t.SupplierId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.ServiceZones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                        CancelationAmount = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
            CreateTable(
                "dbo.ServiceAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        From = c.Int(),
                        Until = c.Int(),
                        Side = c.Int(),
                        CityId = c.Int(nullable: false),
                        ZoneId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCities", t => t.CityId)
                .ForeignKey("dbo.ServiceZones", t => t.ZoneId)
                .Index(t => t.CityId)
                .Index(t => t.ZoneId);
            
            CreateTable(
                "dbo.ServiceCities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProvinceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceProvinces", t => t.ProvinceId)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.ServiceProvinces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCountries", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.ServiceCountries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServicePrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AmountMinute = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountBase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FromTime = c.Time(nullable: false, precision: 7),
                        UntilTime = c.Time(nullable: false, precision: 7),
                        ZoneId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceZones", t => t.ZoneId)
                .Index(t => t.ZoneId);
            
            CreateTable(
                "dbo.ServiceTimeTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromWeekday = c.Int(nullable: false),
                        UntilWeekday = c.Int(nullable: false),
                        FromHour = c.Time(nullable: false, precision: 7),
                        DurationHour = c.Time(nullable: false, precision: 7),
                        ZoneId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceZones", t => t.ZoneId)
                .Index(t => t.ZoneId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.String(nullable: false, maxLength: 3),
                        Reference = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ServiceType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentGateways",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.Payments", "SupplierId", "dbo.ServiceSuppliers");
            DropForeignKey("dbo.ServiceConcessions", "SupplierId", "dbo.ServiceSuppliers");
            DropForeignKey("dbo.ServiceZones", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ServiceTimeTables", "ZoneId", "dbo.ServiceZones");
            DropForeignKey("dbo.ServicePrices", "ZoneId", "dbo.ServiceZones");
            DropForeignKey("dbo.ServiceAddresses", "ZoneId", "dbo.ServiceZones");
            DropForeignKey("dbo.ServiceProvinces", "CountryId", "dbo.ServiceCountries");
            DropForeignKey("dbo.ServiceCities", "ProvinceId", "dbo.ServiceProvinces");
            DropForeignKey("dbo.ServiceAddresses", "CityId", "dbo.ServiceCities");
            DropForeignKey("dbo.Payments", "PaymentMediaId", "dbo.PaymentMedias");
            DropIndex("dbo.ServiceTimeTables", new[] { "ZoneId" });
            DropIndex("dbo.ServicePrices", new[] { "ZoneId" });
            DropIndex("dbo.ServiceProvinces", new[] { "CountryId" });
            DropIndex("dbo.ServiceCities", new[] { "ProvinceId" });
            DropIndex("dbo.ServiceAddresses", new[] { "ZoneId" });
            DropIndex("dbo.ServiceAddresses", new[] { "CityId" });
            DropIndex("dbo.ServiceZones", new[] { "ConcessionId" });
            DropIndex("dbo.ServiceConcessions", new[] { "SupplierId" });
            DropIndex("dbo.Payments", new[] { "SupplierId" });
            DropIndex("dbo.Payments", new[] { "TicketId" });
            DropIndex("dbo.Payments", new[] { "PaymentMediaId" });
            DropTable("dbo.PaymentGateways");
            DropTable("dbo.Tickets");
            DropTable("dbo.ServiceTimeTables");
            DropTable("dbo.ServicePrices");
            DropTable("dbo.ServiceCountries");
            DropTable("dbo.ServiceProvinces");
            DropTable("dbo.ServiceCities");
            DropTable("dbo.ServiceAddresses");
            DropTable("dbo.ServiceZones");
            DropTable("dbo.ServiceConcessions");
            DropTable("dbo.ServiceSuppliers");
            DropTable("dbo.PaymentMedias");
            DropTable("dbo.Payments");
        }
    }
}
