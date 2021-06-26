namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRelationToProductFamilyWithCampaignLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignLineProductFamilies",
                c => new
                    {
                        CampaignLine_Id = c.Int(nullable: false),
                        ProductFamily_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CampaignLine_Id, t.ProductFamily_Id })
                .ForeignKey("dbo.CampaignLines", t => t.CampaignLine_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProductFamilies", t => t.ProductFamily_Id, cascadeDelete: true)
                .Index(t => t.CampaignLine_Id)
                .Index(t => t.ProductFamily_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignLineProductFamilies", "ProductFamily_Id", "dbo.ProductFamilies");
            DropForeignKey("dbo.CampaignLineProductFamilies", "CampaignLine_Id", "dbo.CampaignLines");
            DropIndex("dbo.CampaignLineProductFamilies", new[] { "ProductFamily_Id" });
            DropIndex("dbo.CampaignLineProductFamilies", new[] { "CampaignLine_Id" });
            DropTable("dbo.CampaignLineProductFamilies");
        }
    }
}
