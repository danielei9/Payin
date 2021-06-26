namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableQuantityTransportPrice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportPrices", "Quantity", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportPrices", "Quantity", c => c.Int(nullable: false));
        }
    }
}
