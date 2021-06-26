namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketRefactory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketDetails", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "ZoneId", "dbo.ServiceZones");
            DropForeignKey("dbo.Tickets", "SupplierId", "dbo.ServiceSuppliers");
            DropIndex("dbo.Tickets", new[] { "SupplierId" });
            DropIndex("dbo.Tickets", new[] { "ZoneId" });
            DropIndex("dbo.TicketDetails", new[] { "TicketId" });
            CreateTable(
                "dbo.PaymentsConcessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);

						Sql(
							"DELETE dbo.Tickets"
						);
            AddColumn("dbo.Tickets", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "TaxName", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "TaxAddress", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "TaxNumber", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "ConcessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "ConcessionId");
            AddForeignKey("dbo.Tickets", "ConcessionId", "dbo.PaymentsConcessions", "Id");
            DropColumn("dbo.Tickets", "AddressName");
            DropColumn("dbo.Tickets", "AddressNumber");
            DropColumn("dbo.Tickets", "CityName");
            DropColumn("dbo.Tickets", "ConcessionName");
            DropColumn("dbo.Tickets", "ServiceType");
            DropColumn("dbo.Tickets", "Until");
            DropColumn("dbo.Tickets", "SupplierId");
            DropColumn("dbo.Tickets", "SupplierName");
            DropColumn("dbo.Tickets", "SupplierTaxName");
            DropColumn("dbo.Tickets", "SupplierTaxAddress");
            DropColumn("dbo.Tickets", "SupplierTaxNumber");
            DropColumn("dbo.Tickets", "ZoneId");
            DropColumn("dbo.Tickets", "ZoneName");
            DropColumn("dbo.Payments", "Until");
            DropTable("dbo.TicketDetails");
            DropTable("dbo.PaymentGateways");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PaymentGateways",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(nullable: false),
                        Article = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        VAT = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TicketId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Payments", "Until", c => c.DateTime());
            AddColumn("dbo.Tickets", "ZoneName", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "ZoneId", c => c.Int());
            AddColumn("dbo.Tickets", "SupplierTaxNumber", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "SupplierTaxAddress", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "SupplierTaxName", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "SupplierName", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "SupplierId", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "Until", c => c.DateTime());
            AddColumn("dbo.Tickets", "ServiceType", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "ConcessionName", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "CityName", c => c.String());
            AddColumn("dbo.Tickets", "AddressNumber", c => c.Int());
            AddColumn("dbo.Tickets", "AddressName", c => c.String());
            DropForeignKey("dbo.Tickets", "ConcessionId", "dbo.PaymentsConcessions");
            DropForeignKey("dbo.PaymentsConcessions", "ConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.PaymentsConcessions", new[] { "ConcessionId" });
            DropIndex("dbo.Tickets", new[] { "ConcessionId" });
            DropColumn("dbo.Tickets", "ConcessionId");
            DropColumn("dbo.Tickets", "TaxNumber");
            DropColumn("dbo.Tickets", "TaxAddress");
            DropColumn("dbo.Tickets", "TaxName");
            DropColumn("dbo.Tickets", "Name");
            DropTable("dbo.PaymentsConcessions");
            CreateIndex("dbo.TicketDetails", "TicketId");
            CreateIndex("dbo.Tickets", "ZoneId");
            CreateIndex("dbo.Tickets", "SupplierId");
            AddForeignKey("dbo.Tickets", "SupplierId", "dbo.ServiceSuppliers", "Id");
            AddForeignKey("dbo.Tickets", "ZoneId", "dbo.ServiceZones", "Id");
            AddForeignKey("dbo.TicketDetails", "TicketId", "dbo.Tickets", "Id", cascadeDelete: true);
        }
    }
}
