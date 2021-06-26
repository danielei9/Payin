namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_2AddTransportPriceFK : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PromoPrices", "TransportPriceId");
            AddForeignKey("dbo.PromoPrices", "TransportPriceId", "dbo.TransportPrices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoPrices", "TransportPriceId", "dbo.TransportPrices");
            DropIndex("dbo.PromoPrices", new[] { "TransportPriceId" });
        }
    }
}
