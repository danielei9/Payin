namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Title = c.String(),
                        Acount = c.String(),
                        Type = c.Int(nullable: false),
                        TicketId = c.Int(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId)
                .ForeignKey("dbo.PaymentConcessions", t => t.ConcessionId)
                .Index(t => t.TicketId)
                .Index(t => t.ConcessionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountLines", "ConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.AccountLines", "TicketId", "dbo.Tickets");
            DropIndex("dbo.AccountLines", new[] { "ConcessionId" });
            DropIndex("dbo.AccountLines", new[] { "TicketId" });
            DropTable("dbo.AccountLines");
        }
    }
}
