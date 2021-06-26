namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_AddUnitiesInTransportPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportPrices", "Unities", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportPrices", "Unities");
        }
    }
}
