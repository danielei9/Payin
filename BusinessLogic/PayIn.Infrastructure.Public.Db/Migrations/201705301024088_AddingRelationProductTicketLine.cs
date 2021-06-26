namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRelationProductTicketLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "ProductId", c => c.Int());
            CreateIndex("dbo.TicketLines", "ProductId");
            AddForeignKey("dbo.TicketLines", "ProductId", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLines", "ProductId", "dbo.Products");
            DropIndex("dbo.TicketLines", new[] { "ProductId" });
            DropColumn("dbo.TicketLines", "ProductId");
        }
    }
}
