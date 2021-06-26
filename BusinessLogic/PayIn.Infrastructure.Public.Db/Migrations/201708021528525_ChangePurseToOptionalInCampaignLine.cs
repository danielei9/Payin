namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePurseToOptionalInCampaignLine : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CampaignLines", new[] { "PurseId" });
            AlterColumn("dbo.CampaignLines", "PurseId", c => c.Int());
            CreateIndex("dbo.CampaignLines", "PurseId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CampaignLines", new[] { "PurseId" });
            AlterColumn("dbo.CampaignLines", "PurseId", c => c.Int(nullable: false));
            CreateIndex("dbo.CampaignLines", "PurseId");
        }
    }
}
