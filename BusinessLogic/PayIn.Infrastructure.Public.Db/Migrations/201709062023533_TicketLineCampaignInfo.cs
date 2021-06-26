namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketLineCampaignInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "CampaignLineType", c => c.Int());
            AddColumn("dbo.TicketLines", "CampaignLineQuantity", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.TicketLines", "CampaignLineId", c => c.Int());
            CreateIndex("dbo.TicketLines", "CampaignLineId");
            AddForeignKey("dbo.TicketLines", "CampaignLineId", "dbo.CampaignLines", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLines", "CampaignLineId", "dbo.CampaignLines");
            DropIndex("dbo.TicketLines", new[] { "CampaignLineId" });
            DropColumn("dbo.TicketLines", "CampaignLineId");
            DropColumn("dbo.TicketLines", "CampaignLineQuantity");
            DropColumn("dbo.TicketLines", "CampaignLineType");
        }
    }
}
