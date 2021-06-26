namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignLineUsersAndGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignLineServiceGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignLineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignLines", t => t.CampaignLineId)
                .Index(t => t.CampaignLineId);
            
            CreateTable(
                "dbo.CampaignLineServiceUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignLineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignLines", t => t.CampaignLineId)
                .Index(t => t.CampaignLineId);
            
            AddColumn("dbo.ServiceUsers", "CampaignLineServiceUser_Id", c => c.Int());
            AddColumn("dbo.ServiceGroups", "CampaignLineServiceGroup_Id", c => c.Int());
            CreateIndex("dbo.ServiceUsers", "CampaignLineServiceUser_Id");
            CreateIndex("dbo.ServiceGroups", "CampaignLineServiceGroup_Id");
            AddForeignKey("dbo.ServiceGroups", "CampaignLineServiceGroup_Id", "dbo.CampaignLineServiceGroups", "Id");
            AddForeignKey("dbo.ServiceUsers", "CampaignLineServiceUser_Id", "dbo.CampaignLineServiceUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignLineServiceUsers", "CampaignLineId", "dbo.CampaignLines");
            DropForeignKey("dbo.ServiceUsers", "CampaignLineServiceUser_Id", "dbo.CampaignLineServiceUsers");
            DropForeignKey("dbo.CampaignLineServiceGroups", "CampaignLineId", "dbo.CampaignLines");
            DropForeignKey("dbo.ServiceGroups", "CampaignLineServiceGroup_Id", "dbo.CampaignLineServiceGroups");
            DropIndex("dbo.CampaignLineServiceUsers", new[] { "CampaignLineId" });
            DropIndex("dbo.CampaignLineServiceGroups", new[] { "CampaignLineId" });
            DropIndex("dbo.ServiceGroups", new[] { "CampaignLineServiceGroup_Id" });
            DropIndex("dbo.ServiceUsers", new[] { "CampaignLineServiceUser_Id" });
            DropColumn("dbo.ServiceGroups", "CampaignLineServiceGroup_Id");
            DropColumn("dbo.ServiceUsers", "CampaignLineServiceUser_Id");
            DropTable("dbo.CampaignLineServiceUsers");
            DropTable("dbo.CampaignLineServiceGroups");
        }
    }
}
