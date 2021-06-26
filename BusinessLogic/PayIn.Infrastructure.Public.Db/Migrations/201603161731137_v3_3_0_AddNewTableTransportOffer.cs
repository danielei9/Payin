namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_AddNewTableTransportOffer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportOffers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        TransportPriceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportPrices", t => t.TransportPriceId)
                .Index(t => t.TransportPriceId);
            
            AddColumn("dbo.TransportPrices", "TemporalUnities", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportOffers", "TransportPriceId", "dbo.TransportPrices");
            DropIndex("dbo.TransportOffers", new[] { "TransportPriceId" });
            DropColumn("dbo.TransportPrices", "TemporalUnities");
            DropTable("dbo.TransportOffers");
        }
    }
}
