namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_1_0_ChangeRechargeAndChangePaymentConcessionCampaign : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId", "dbo.PaymentConcessions");
            DropIndex("dbo.PaymentConcessionPurses", new[] { "PurseId" });
            DropIndex("dbo.PaymentConcessionCampaigns", new[] { "PaymentConcessionFollowId" });
            DropIndex("dbo.PaymentConcessionCampaigns", new[] { "CampaignId" });
            AddColumn("dbo.Purses", "State", c => c.Int(nullable: false));
            AddColumn("dbo.Recharges", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Recharges", "UserName", c => c.String(nullable: false));
            AddColumn("dbo.Recharges", "UserLogin", c => c.String(nullable: false));
            AddColumn("dbo.Recharges", "TaxName", c => c.String(nullable: false));
            AddColumn("dbo.Recharges", "TaxAddress", c => c.String(nullable: false));
            AddColumn("dbo.Recharges", "TaxNumber", c => c.String(nullable: false));
            AlterColumn("dbo.PaymentConcessionPurses", "PurseId", c => c.Int(nullable: false));
            AlterColumn("dbo.PaymentConcessionCampaigns", "CampaignId", c => c.Int(nullable: false));
            CreateIndex("dbo.PaymentConcessionPurses", "PurseId");
            CreateIndex("dbo.PaymentConcessionCampaigns", "CampaignId");
            DropColumn("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId", c => c.Int(nullable: false));
            DropIndex("dbo.PaymentConcessionCampaigns", new[] { "CampaignId" });
            DropIndex("dbo.PaymentConcessionPurses", new[] { "PurseId" });
            AlterColumn("dbo.PaymentConcessionCampaigns", "CampaignId", c => c.Int());
            AlterColumn("dbo.PaymentConcessionPurses", "PurseId", c => c.Int());
            DropColumn("dbo.Recharges", "TaxNumber");
            DropColumn("dbo.Recharges", "TaxAddress");
            DropColumn("dbo.Recharges", "TaxName");
            DropColumn("dbo.Recharges", "UserLogin");
            DropColumn("dbo.Recharges", "UserName");
            DropColumn("dbo.Recharges", "Date");
            DropColumn("dbo.Purses", "State");
            CreateIndex("dbo.PaymentConcessionCampaigns", "CampaignId");
            CreateIndex("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId");
            CreateIndex("dbo.PaymentConcessionPurses", "PurseId");
            AddForeignKey("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId", "dbo.PaymentConcessions", "Id");
        }
    }
}
