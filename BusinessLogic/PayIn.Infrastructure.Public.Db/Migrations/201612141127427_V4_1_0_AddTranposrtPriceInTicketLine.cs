namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V4_1_0_AddTranposrtPriceInTicketLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "TransportPriceId", c => c.Int(nullable:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketLines", "TransportPriceId");
        }
    }
}
