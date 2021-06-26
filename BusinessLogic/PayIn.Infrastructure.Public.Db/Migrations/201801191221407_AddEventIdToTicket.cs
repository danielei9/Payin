namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventIdToTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "EventId", c => c.Int());
            CreateIndex("dbo.Tickets", "EventId");
            AddForeignKey("dbo.Tickets", "EventId", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "EventId", "dbo.Events");
            DropIndex("dbo.Tickets", new[] { "EventId" });
            DropColumn("dbo.Tickets", "EventId");
        }
    }
}
