namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_3_3_0_CreateEntityTicketLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TicketLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Reference = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TicketId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.TicketId)
                .Index(t => t.TicketId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLines", "TicketId", "dbo.Tickets");
            DropIndex("dbo.TicketLines", new[] { "TicketId" });
            DropTable("dbo.TicketLines");
        }
    }
}
