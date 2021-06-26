namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaingLineUpgrade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceGroups", "CampaignLineServiceGroup_Id", "dbo.CampaignLineServiceGroups");
            DropForeignKey("dbo.ServiceUsers", "CampaignLineServiceUser_Id", "dbo.CampaignLineServiceUsers");
            DropIndex("dbo.ServiceUsers", new[] { "CampaignLineServiceUser_Id" });
            DropIndex("dbo.ServiceGroups", new[] { "CampaignLineServiceGroup_Id" });
            AddColumn("dbo.CampaignLineServiceGroups", "ServiceGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.CampaignLineServiceUsers", "ServiceUserId", c => c.Int(nullable: false));
            CreateIndex("dbo.CampaignLineServiceGroups", "ServiceGroupId");
            CreateIndex("dbo.CampaignLineServiceUsers", "ServiceUserId");
            AddForeignKey("dbo.CampaignLineServiceGroups", "ServiceGroupId", "dbo.ServiceGroups", "Id");
            AddForeignKey("dbo.CampaignLineServiceUsers", "ServiceUserId", "dbo.ServiceUsers", "Id");
            DropColumn("dbo.ServiceUsers", "CampaignLineServiceUser_Id");
            DropColumn("dbo.ServiceGroups", "CampaignLineServiceGroup_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceGroups", "CampaignLineServiceGroup_Id", c => c.Int());
            AddColumn("dbo.ServiceUsers", "CampaignLineServiceUser_Id", c => c.Int());
            DropForeignKey("dbo.CampaignLineServiceUsers", "ServiceUserId", "dbo.ServiceUsers");
            DropForeignKey("dbo.CampaignLineServiceGroups", "ServiceGroupId", "dbo.ServiceGroups");
            DropIndex("dbo.CampaignLineServiceUsers", new[] { "ServiceUserId" });
            DropIndex("dbo.CampaignLineServiceGroups", new[] { "ServiceGroupId" });
            DropColumn("dbo.CampaignLineServiceUsers", "ServiceUserId");
            DropColumn("dbo.CampaignLineServiceGroups", "ServiceGroupId");
            CreateIndex("dbo.ServiceGroups", "CampaignLineServiceGroup_Id");
            CreateIndex("dbo.ServiceUsers", "CampaignLineServiceUser_Id");
            AddForeignKey("dbo.ServiceUsers", "CampaignLineServiceUser_Id", "dbo.CampaignLineServiceUsers", "Id");
            AddForeignKey("dbo.ServiceGroups", "CampaignLineServiceGroup_Id", "dbo.CampaignLineServiceGroups", "Id");
        }
    }
}
