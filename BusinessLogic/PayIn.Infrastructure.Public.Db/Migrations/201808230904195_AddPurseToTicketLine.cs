namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurseToTicketLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "PurseId", c => c.Int());
            CreateIndex("dbo.TicketLines", "PurseId");
            AddForeignKey("dbo.TicketLines", "PurseId", "dbo.Purses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLines", "PurseId", "dbo.Purses");
            DropIndex("dbo.TicketLines", new[] { "PurseId" });
            DropColumn("dbo.TicketLines", "PurseId");
        }
    }
}
