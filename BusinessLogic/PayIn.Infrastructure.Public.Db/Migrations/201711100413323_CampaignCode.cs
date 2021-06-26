namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Long(nullable: false),
                        State = c.Int(nullable: false),
                        Login = c.String(nullable: false),
                        CampaignId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CampaignCodes", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.CampaignCodes", new[] { "CampaignId" });
            DropTable("dbo.CampaignCodes");
        }
    }
}
