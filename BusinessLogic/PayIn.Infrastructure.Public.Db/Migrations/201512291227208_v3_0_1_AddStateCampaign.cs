namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_0_1_AddStateCampaign : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CampaignLines", new[] { "PurseId" });
            AddColumn("dbo.Campaigns", "State", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignLines", "PurseId", c => c.Int());
            CreateIndex("dbo.CampaignLines", "PurseId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CampaignLines", new[] { "PurseId" });
            AlterColumn("dbo.CampaignLines", "PurseId", c => c.Int(nullable: false));
            DropColumn("dbo.Campaigns", "State");
            CreateIndex("dbo.CampaignLines", "PurseId");
        }
    }
}
