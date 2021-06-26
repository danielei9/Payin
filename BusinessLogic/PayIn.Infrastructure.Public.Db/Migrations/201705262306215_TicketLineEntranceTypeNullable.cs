namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketLineEntranceTypeNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TicketLines", new[] { "EntranceTypeId" });
            AlterColumn("dbo.TicketLines", "EntranceTypeId", c => c.Int());
            CreateIndex("dbo.TicketLines", "EntranceTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TicketLines", new[] { "EntranceTypeId" });
            AlterColumn("dbo.TicketLines", "EntranceTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.TicketLines", "EntranceTypeId");
        }
    }
}
