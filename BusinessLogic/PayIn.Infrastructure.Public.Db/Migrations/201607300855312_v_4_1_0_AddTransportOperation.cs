namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddTransportOperation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportOperations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        OperationType = c.Int(nullable: false),
                        OperationDate = c.DateTime(nullable: false),
                        ConfirmationRechargeDate = c.DateTime(),
                        DateTimeValue = c.DateTime(),
                        DateTimeValueOld = c.DateTime(),
                        QuantityValue = c.Int(),
                        QuantityValueOld = c.Int(),
                        StringValue = c.String(nullable: false),
                        StringValueOld = c.String(nullable: false),
                        TransportPriceId = c.Int(),
                        TicketLineId = c.Int(),
                        LogId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Logs", t => t.LogId)
                .ForeignKey("dbo.TicketLines", t => t.TicketLineId)
                .ForeignKey("dbo.TransportPrices", t => t.TransportPriceId)
                .Index(t => t.TransportPriceId)
                .Index(t => t.TicketLineId)
                .Index(t => t.LogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportOperations", "TransportPriceId", "dbo.TransportPrices");
            DropForeignKey("dbo.TransportOperations", "TicketLineId", "dbo.TicketLines");
            DropForeignKey("dbo.TransportOperations", "LogId", "dbo.Logs");
            DropIndex("dbo.TransportOperations", new[] { "LogId" });
            DropIndex("dbo.TransportOperations", new[] { "TicketLineId" });
            DropIndex("dbo.TransportOperations", new[] { "TransportPriceId" });
            DropTable("dbo.TransportOperations");
        }
    }
}
