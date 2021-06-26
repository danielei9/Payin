namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletingDuplicatedRelantionInCampaignWithSystemCard : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Campaigns", "SystemCardId", "dbo.ServiceConcessions");
            DropIndex("dbo.Campaigns", new[] { "SystemCardId" });
            DropColumn("dbo.Campaigns", "SystemCardId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "SystemCardId", c => c.Int());
            CreateIndex("dbo.Campaigns", "SystemCardId");
            AddForeignKey("dbo.Campaigns", "SystemCardId", "dbo.ServiceConcessions", "Id");
        }
    }
}
