namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntranceWithoutTicketLine : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Entrances", new[] { "TicketLineId" });
            AlterColumn("dbo.Entrances", "TicketLineId", c => c.Int());
            CreateIndex("dbo.Entrances", "TicketLineId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Entrances", new[] { "TicketLineId" });
            AlterColumn("dbo.Entrances", "TicketLineId", c => c.Int(nullable: false));
            CreateIndex("dbo.Entrances", "TicketLineId");
        }
    }
}
