namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingReflexiveRelationToTicketLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "FromId", c => c.Int());
            CreateIndex("dbo.TicketLines", "FromId");
            AddForeignKey("dbo.TicketLines", "FromId", "dbo.TicketLines", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLines", "FromId", "dbo.TicketLines");
            DropIndex("dbo.TicketLines", new[] { "FromId" });
            DropColumn("dbo.TicketLines", "FromId");
        }
    }
}
