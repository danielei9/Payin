namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketRelatedWithCampaignAndCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "CampaignCode", c => c.Long());
            AddColumn("dbo.TicketLines", "CampaignId", c => c.Int());
            CreateIndex("dbo.TicketLines", "CampaignId");
            AddForeignKey("dbo.TicketLines", "CampaignId", "dbo.Campaigns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLines", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.TicketLines", new[] { "CampaignId" });
            DropColumn("dbo.TicketLines", "CampaignId");
            DropColumn("dbo.TicketLines", "CampaignCode");
        }
    }
}
