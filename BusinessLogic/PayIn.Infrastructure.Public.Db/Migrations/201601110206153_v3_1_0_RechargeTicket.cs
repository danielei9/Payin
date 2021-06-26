namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_1_0_RechargeTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recharges", "TicketId", c => c.Int(nullable: false));
            CreateIndex("dbo.Recharges", "TicketId");
            AddForeignKey("dbo.Recharges", "TicketId", "dbo.Tickets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recharges", "TicketId", "dbo.Tickets");
            DropIndex("dbo.Recharges", new[] { "TicketId" });
            DropColumn("dbo.Recharges", "TicketId");
        }
    }
}
