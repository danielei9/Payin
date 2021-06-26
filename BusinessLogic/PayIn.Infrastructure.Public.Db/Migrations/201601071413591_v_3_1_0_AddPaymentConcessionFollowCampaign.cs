namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_3_1_0_AddPaymentConcessionFollowCampaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId", c => c.Int(nullable: false));
            CreateIndex("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId");
            AddForeignKey("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId", "dbo.PaymentConcessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId", "dbo.PaymentConcessions");
            DropIndex("dbo.PaymentConcessionCampaigns", new[] { "PaymentConcessionFollowId" });
            DropColumn("dbo.PaymentConcessionCampaigns", "PaymentConcessionFollowId");
        }
    }
}
