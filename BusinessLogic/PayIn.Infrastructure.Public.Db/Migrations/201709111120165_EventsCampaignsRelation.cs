namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventsCampaignsRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventCampaigns",
                c => new
                    {
                        Event_Id = c.Int(nullable: false),
                        Campaign_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Campaign_Id })
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.Campaigns", t => t.Campaign_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.Campaign_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventCampaigns", "Campaign_Id", "dbo.Campaigns");
            DropForeignKey("dbo.EventCampaigns", "Event_Id", "dbo.Events");
            DropIndex("dbo.EventCampaigns", new[] { "Campaign_Id" });
            DropIndex("dbo.EventCampaigns", new[] { "Event_Id" });
            DropTable("dbo.EventCampaigns");
        }
    }
}
