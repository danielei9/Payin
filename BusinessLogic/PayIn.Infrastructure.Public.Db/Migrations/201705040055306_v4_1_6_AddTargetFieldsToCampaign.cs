namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_6_AddTargetFieldsToCampaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "TargetSystemCardId", c => c.Int());
            AddColumn("dbo.Campaigns", "TargetConcessionId", c => c.Int());
            CreateIndex("dbo.Campaigns", "TargetSystemCardId");
            CreateIndex("dbo.Campaigns", "TargetConcessionId");
            AddForeignKey("dbo.Campaigns", "TargetConcessionId", "dbo.ServiceConcessions", "Id");
            AddForeignKey("dbo.Campaigns", "TargetSystemCardId", "dbo.SystemCards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "TargetSystemCardId", "dbo.SystemCards");
            DropForeignKey("dbo.Campaigns", "TargetConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.Campaigns", new[] { "TargetConcessionId" });
            DropIndex("dbo.Campaigns", new[] { "TargetSystemCardId" });
            DropColumn("dbo.Campaigns", "TargetConcessionId");
            DropColumn("dbo.Campaigns", "TargetSystemCardId");
        }
    }
}
