namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0AddBalanceUnitiesInTransportPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportPrices", "BalanceUnities", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportPrices", "BalanceUnities");
        }
    }
}
