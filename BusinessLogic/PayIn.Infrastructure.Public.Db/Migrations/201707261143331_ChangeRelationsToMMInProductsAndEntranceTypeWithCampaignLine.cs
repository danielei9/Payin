namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRelationsToMMInProductsAndEntranceTypeWithCampaignLine : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EntranceTypes", "CampaignLineId", "dbo.CampaignLines");
            DropForeignKey("dbo.Products", "CampaignLineId", "dbo.CampaignLines");
            DropIndex("dbo.EntranceTypes", new[] { "CampaignLineId" });
            DropIndex("dbo.Products", new[] { "CampaignLineId" });
            CreateTable(
                "dbo.CampaignLineEntranceTypes",
                c => new
                    {
                        CampaignLine_Id = c.Int(nullable: false),
                        EntranceType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CampaignLine_Id, t.EntranceType_Id })
                .ForeignKey("dbo.CampaignLines", t => t.CampaignLine_Id, cascadeDelete: true)
                .ForeignKey("dbo.EntranceTypes", t => t.EntranceType_Id, cascadeDelete: true)
                .Index(t => t.CampaignLine_Id)
                .Index(t => t.EntranceType_Id);
            
            CreateTable(
                "dbo.CampaignLineProducts",
                c => new
                    {
                        CampaignLine_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CampaignLine_Id, t.Product_Id })
                .ForeignKey("dbo.CampaignLines", t => t.CampaignLine_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.CampaignLine_Id)
                .Index(t => t.Product_Id);
            
            DropColumn("dbo.EntranceTypes", "CampaignLineId");
            DropColumn("dbo.Products", "CampaignLineId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CampaignLineId", c => c.Int());
            AddColumn("dbo.EntranceTypes", "CampaignLineId", c => c.Int());
            DropForeignKey("dbo.CampaignLineProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.CampaignLineProducts", "CampaignLine_Id", "dbo.CampaignLines");
            DropForeignKey("dbo.CampaignLineEntranceTypes", "EntranceType_Id", "dbo.EntranceTypes");
            DropForeignKey("dbo.CampaignLineEntranceTypes", "CampaignLine_Id", "dbo.CampaignLines");
            DropIndex("dbo.CampaignLineProducts", new[] { "Product_Id" });
            DropIndex("dbo.CampaignLineProducts", new[] { "CampaignLine_Id" });
            DropIndex("dbo.CampaignLineEntranceTypes", new[] { "EntranceType_Id" });
            DropIndex("dbo.CampaignLineEntranceTypes", new[] { "CampaignLine_Id" });
            DropTable("dbo.CampaignLineProducts");
            DropTable("dbo.CampaignLineEntranceTypes");
            CreateIndex("dbo.Products", "CampaignLineId");
            CreateIndex("dbo.EntranceTypes", "CampaignLineId");
            AddForeignKey("dbo.Products", "CampaignLineId", "dbo.CampaignLines", "Id");
            AddForeignKey("dbo.EntranceTypes", "CampaignLineId", "dbo.CampaignLines", "Id");
        }
    }
}
