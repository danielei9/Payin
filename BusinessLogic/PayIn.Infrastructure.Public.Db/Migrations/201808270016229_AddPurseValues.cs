namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurseValues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurseValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Slot = c.Int(),
                        Seq = c.Int(),
                        PurseId = c.Int(nullable: false),
                        TicketId = c.Int(),
                        ServiceCardId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCards", t => t.ServiceCardId)
                .ForeignKey("dbo.Tickets", t => t.TicketId)
                .ForeignKey("dbo.Purses", t => t.PurseId)
                .Index(t => t.PurseId)
                .Index(t => t.TicketId)
                .Index(t => t.ServiceCardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurseValues", "PurseId", "dbo.Purses");
            DropForeignKey("dbo.PurseValues", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.PurseValues", "ServiceCardId", "dbo.ServiceCards");
            DropIndex("dbo.PurseValues", new[] { "ServiceCardId" });
            DropIndex("dbo.PurseValues", new[] { "TicketId" });
            DropIndex("dbo.PurseValues", new[] { "PurseId" });
            DropTable("dbo.PurseValues");
        }
    }
}
