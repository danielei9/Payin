namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddEntityPromoPrice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotionId = c.Int(nullable: false),
                        TransportPriceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promotions", t => t.PromotionId)
                .Index(t => t.PromotionId);
            
            DropColumn("dbo.PromoConditions", "TransportTitleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoConditions", "TransportTitleId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PromoPrices", "PromotionId", "dbo.Promotions");
            DropIndex("dbo.PromoPrices", new[] { "PromotionId" });
            DropTable("dbo.PromoPrices");
        }
    }
}
