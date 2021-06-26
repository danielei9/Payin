namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWalletNumberToTicketLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "WalletNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketLines", "WalletNumber");
        }
    }
}
