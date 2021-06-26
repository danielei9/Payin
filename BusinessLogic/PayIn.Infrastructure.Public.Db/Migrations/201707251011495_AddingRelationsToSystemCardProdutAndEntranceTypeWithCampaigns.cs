namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRelationsToSystemCardProdutAndEntranceTypeWithCampaigns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "SystemCardId", c => c.Int());
            AddColumn("dbo.EntranceTypes", "CampaignLineId", c => c.Int());
            AddColumn("dbo.Products", "CampaignLineId", c => c.Int());
            CreateIndex("dbo.Campaigns", "SystemCardId");
            CreateIndex("dbo.EntranceTypes", "CampaignLineId");
            CreateIndex("dbo.Products", "CampaignLineId");
            AddForeignKey("dbo.EntranceTypes", "CampaignLineId", "dbo.CampaignLines", "Id");
            AddForeignKey("dbo.Products", "CampaignLineId", "dbo.CampaignLines", "Id");
            AddForeignKey("dbo.Campaigns", "SystemCardId", "dbo.ServiceConcessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "SystemCardId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.Products", "CampaignLineId", "dbo.CampaignLines");
            DropForeignKey("dbo.EntranceTypes", "CampaignLineId", "dbo.CampaignLines");
            DropIndex("dbo.Products", new[] { "CampaignLineId" });
            DropIndex("dbo.EntranceTypes", new[] { "CampaignLineId" });
            DropIndex("dbo.Campaigns", new[] { "SystemCardId" });
            DropColumn("dbo.Products", "CampaignLineId");
            DropColumn("dbo.EntranceTypes", "CampaignLineId");
            DropColumn("dbo.Campaigns", "SystemCardId");
        }
    }
}
