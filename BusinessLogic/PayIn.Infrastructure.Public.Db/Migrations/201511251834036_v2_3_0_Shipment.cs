namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_0_Shipment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
            AddColumn("dbo.Tickets", "PaymentUserId", c => c.Int());
            AddColumn("dbo.Tickets", "ShipmentId", c => c.Int());
            CreateIndex("dbo.Tickets", "PaymentUserId");
            CreateIndex("dbo.Tickets", "ShipmentId");
            AddForeignKey("dbo.Tickets", "ShipmentId", "dbo.Shipments", "Id");
            AddForeignKey("dbo.Tickets", "PaymentUserId", "dbo.PaymentUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shipments", "ConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.Tickets", "PaymentUserId", "dbo.PaymentUsers");
            DropForeignKey("dbo.Tickets", "ShipmentId", "dbo.Shipments");
            DropIndex("dbo.Shipments", new[] { "ConcessionId" });
            DropIndex("dbo.Tickets", new[] { "ShipmentId" });
            DropIndex("dbo.Tickets", new[] { "PaymentUserId" });
            DropColumn("dbo.Tickets", "ShipmentId");
            DropColumn("dbo.Tickets", "PaymentUserId");
            DropTable("dbo.Shipments");
        }
    }
}
