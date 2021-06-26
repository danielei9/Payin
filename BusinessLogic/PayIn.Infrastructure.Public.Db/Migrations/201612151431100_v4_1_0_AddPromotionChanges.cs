namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_AddPromotionChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromoExecutions", "TicketId", "dbo.Tickets");
            DropIndex("dbo.PromoExecutions", new[] { "TicketId" });
            AddColumn("dbo.Promotions", "ConcessionId", c => c.Int(nullable: true));
            AddColumn("dbo.PromoExecutions", "AppliedDate", c => c.DateTime());
            AddColumn("dbo.PromoExecutions", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PromoExecutions", "State", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.PromoExecutions", "SerialNumber", c => c.String());
            AddColumn("dbo.PromoExecutions", "TicketLineId", c => c.Int());
            CreateIndex("dbo.Promotions", "ConcessionId");
            CreateIndex("dbo.PromoExecutions", "TicketLineId");
            AddForeignKey("dbo.PromoExecutions", "TicketLineId", "dbo.TicketLines", "Id");
            AddForeignKey("dbo.Promotions", "ConcessionId", "dbo.PaymentConcessions", "Id");
            DropColumn("dbo.PromoExecutions", "Applied");
            DropColumn("dbo.PromoExecutions", "Date");
            DropColumn("dbo.PromoExecutions", "TicketId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoExecutions", "TicketId", c => c.Int());
            AddColumn("dbo.PromoExecutions", "Date", c => c.DateTime());
            AddColumn("dbo.PromoExecutions", "Applied", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Promotions", "ConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.PromoExecutions", "TicketLineId", "dbo.TicketLines");
            DropIndex("dbo.PromoExecutions", new[] { "TicketLineId" });
            DropIndex("dbo.Promotions", new[] { "ConcessionId" });
            DropColumn("dbo.PromoExecutions", "TicketLineId");
            DropColumn("dbo.PromoExecutions", "SerialNumber");
            DropColumn("dbo.PromoExecutions", "State");
            DropColumn("dbo.PromoExecutions", "Cost");
            DropColumn("dbo.PromoExecutions", "AppliedDate");
            DropColumn("dbo.Promotions", "ConcessionId");
            CreateIndex("dbo.PromoExecutions", "TicketId");
            AddForeignKey("dbo.PromoExecutions", "TicketId", "dbo.Tickets", "Id");
        }
    }
}
