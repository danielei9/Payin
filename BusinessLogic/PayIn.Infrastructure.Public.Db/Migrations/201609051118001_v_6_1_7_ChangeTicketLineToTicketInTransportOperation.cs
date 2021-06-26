namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_6_1_7_ChangeTicketLineToTicketInTransportOperation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TransportOperations", "LogId", "dbo.Logs");
            DropForeignKey("dbo.TransportOperations", "TicketLineId", "dbo.TicketLines");
            DropIndex("dbo.TransportOperations", new[] { "TicketLineId" });
            DropIndex("dbo.TransportOperations", new[] { "LogId" });
            AddColumn("dbo.TransportOperations", "TicketId", c => c.Int());
            CreateIndex("dbo.TransportOperations", "TicketId");
            AddForeignKey("dbo.TransportOperations", "TicketId", "dbo.Tickets", "Id");
            DropColumn("dbo.TransportOperations", "TicketLineId");
            DropColumn("dbo.TransportOperations", "LogId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransportOperations", "LogId", c => c.Int());
            AddColumn("dbo.TransportOperations", "TicketLineId", c => c.Int());
            DropForeignKey("dbo.TransportOperations", "TicketId", "dbo.Tickets");
            DropIndex("dbo.TransportOperations", new[] { "TicketId" });
            DropColumn("dbo.TransportOperations", "TicketId");
            CreateIndex("dbo.TransportOperations", "LogId");
            CreateIndex("dbo.TransportOperations", "TicketLineId");
            AddForeignKey("dbo.TransportOperations", "TicketLineId", "dbo.TicketLines", "Id");
            AddForeignKey("dbo.TransportOperations", "LogId", "dbo.Logs", "Id");
        }
    }
}
