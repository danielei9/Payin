namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceOperation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PurseValues", "ServiceCardId", "dbo.ServiceCards");
            DropForeignKey("dbo.PurseValues", "TicketId", "dbo.Tickets");
            DropIndex("dbo.PurseValues", new[] { "TicketId" });
            DropIndex("dbo.PurseValues", new[] { "ServiceCardId" });
            CreateTable(
                "dbo.ServiceOperations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Seq = c.Int(),
                        ESeq = c.Int(),
                        CardId = c.Int(nullable: false),
                        Ticket_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCards", t => t.CardId)
                .ForeignKey("dbo.Tickets", t => t.Ticket_Id)
                .Index(t => t.CardId)
                .Index(t => t.Ticket_Id);
            
            AddColumn("dbo.PurseValues", "ServiceOperationId", c => c.Int(nullable: false));
            CreateIndex("dbo.PurseValues", "ServiceOperationId");
            AddForeignKey("dbo.PurseValues", "ServiceOperationId", "dbo.ServiceOperations", "Id");
            DropColumn("dbo.PurseValues", "Date");
            DropColumn("dbo.PurseValues", "Seq");
            DropColumn("dbo.PurseValues", "TicketId");
            DropColumn("dbo.PurseValues", "ServiceCardId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PurseValues", "ServiceCardId", c => c.Int());
            AddColumn("dbo.PurseValues", "TicketId", c => c.Int());
            AddColumn("dbo.PurseValues", "Seq", c => c.Int());
            AddColumn("dbo.PurseValues", "Date", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.PurseValues", "ServiceOperationId", "dbo.ServiceOperations");
            DropForeignKey("dbo.ServiceOperations", "Ticket_Id", "dbo.Tickets");
            DropForeignKey("dbo.ServiceOperations", "CardId", "dbo.ServiceCards");
            DropIndex("dbo.PurseValues", new[] { "ServiceOperationId" });
            DropIndex("dbo.ServiceOperations", new[] { "Ticket_Id" });
            DropIndex("dbo.ServiceOperations", new[] { "CardId" });
            DropColumn("dbo.PurseValues", "ServiceOperationId");
            DropTable("dbo.ServiceOperations");
            CreateIndex("dbo.PurseValues", "ServiceCardId");
            CreateIndex("dbo.PurseValues", "TicketId");
            AddForeignKey("dbo.PurseValues", "TicketId", "dbo.Tickets", "Id");
            AddForeignKey("dbo.PurseValues", "ServiceCardId", "dbo.ServiceCards", "Id");
        }
    }
}
