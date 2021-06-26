namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUidToTicketLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "Uid", c => c.Long());
            DropColumn("dbo.TicketLines", "WalletNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketLines", "WalletNumber", c => c.Int());
            DropColumn("dbo.TicketLines", "Uid");
        }
    }
}
