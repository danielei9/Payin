namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_1_0_AddPurseStructure : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentConcessions", "OwnerCampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.PaymentConcessions", "PayCampaignId", "dbo.Campaigns");
            DropIndex("dbo.PaymentConcessions", new[] { "OwnerCampaignId" });
            DropIndex("dbo.PaymentConcessions", new[] { "PayCampaignId" });
            DropIndex("dbo.CampaignLines", new[] { "PurseId" });
            CreateTable(
                "dbo.PaymentConcessionPurses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        PaymentConcessionId = c.Int(nullable: false),
                        PurseId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Purses", t => t.PurseId)
                .ForeignKey("dbo.PaymentConcessions", t => t.PaymentConcessionId)
                .Index(t => t.PaymentConcessionId)
                .Index(t => t.PurseId);
            
            CreateTable(
                "dbo.Recharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LiquidationId = c.Int(),
                        PaymentMediaId = c.Int(nullable: false),
                        CampaignLineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentMedias", t => t.PaymentMediaId)
                .ForeignKey("dbo.CampaignLines", t => t.CampaignLineId)
                .ForeignKey("dbo.Liquidations", t => t.LiquidationId)
                .Index(t => t.LiquidationId)
                .Index(t => t.PaymentMediaId)
                .Index(t => t.CampaignLineId);
            
            CreateTable(
                "dbo.PaymentConcessionCampaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        PaymentConcessionId = c.Int(nullable: false),
                        CampaignId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.PaymentConcessions", t => t.PaymentConcessionId)
                .Index(t => t.PaymentConcessionId)
                .Index(t => t.CampaignId);
            
            AddColumn("dbo.Campaigns", "ConcessionId", c => c.Int(nullable: false));
            AddColumn("dbo.CampaignLines", "State", c => c.Int(nullable: false));
            AddColumn("dbo.Purses", "Name", c => c.String());
            AlterColumn("dbo.CampaignLines", "PurseId", c => c.Int(nullable: false));
            AlterColumn("dbo.PaymentMedias", "VisualOrder", c => c.Int());
            CreateIndex("dbo.Campaigns", "ConcessionId");
            CreateIndex("dbo.CampaignLines", "PurseId");
            AddForeignKey("dbo.Campaigns", "ConcessionId", "dbo.PaymentConcessions", "Id");
            DropColumn("dbo.PaymentConcessions", "OwnerCampaignId");
            DropColumn("dbo.PaymentConcessions", "PayCampaignId");
            DropColumn("dbo.PaymentMedias", "Balance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentMedias", "Balance", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PaymentConcessions", "PayCampaignId", c => c.Int());
            AddColumn("dbo.PaymentConcessions", "OwnerCampaignId", c => c.Int());
            DropForeignKey("dbo.Recharges", "LiquidationId", "dbo.Liquidations");
            DropForeignKey("dbo.PaymentConcessionPurses", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.PaymentConcessionCampaigns", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.Campaigns", "ConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.PaymentConcessionCampaigns", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Recharges", "CampaignLineId", "dbo.CampaignLines");
            DropForeignKey("dbo.Recharges", "PaymentMediaId", "dbo.PaymentMedias");
            DropForeignKey("dbo.PaymentConcessionPurses", "PurseId", "dbo.Purses");
            DropIndex("dbo.PaymentConcessionCampaigns", new[] { "CampaignId" });
            DropIndex("dbo.PaymentConcessionCampaigns", new[] { "PaymentConcessionId" });
            DropIndex("dbo.Recharges", new[] { "CampaignLineId" });
            DropIndex("dbo.Recharges", new[] { "PaymentMediaId" });
            DropIndex("dbo.Recharges", new[] { "LiquidationId" });
            DropIndex("dbo.PaymentConcessionPurses", new[] { "PurseId" });
            DropIndex("dbo.PaymentConcessionPurses", new[] { "PaymentConcessionId" });
            DropIndex("dbo.CampaignLines", new[] { "PurseId" });
            DropIndex("dbo.Campaigns", new[] { "ConcessionId" });
            AlterColumn("dbo.PaymentMedias", "VisualOrder", c => c.Int(nullable: false));
            AlterColumn("dbo.CampaignLines", "PurseId", c => c.Int());
            DropColumn("dbo.Purses", "Name");
            DropColumn("dbo.CampaignLines", "State");
            DropColumn("dbo.Campaigns", "ConcessionId");
            DropTable("dbo.PaymentConcessionCampaigns");
            DropTable("dbo.Recharges");
            DropTable("dbo.PaymentConcessionPurses");
            CreateIndex("dbo.CampaignLines", "PurseId");
            CreateIndex("dbo.PaymentConcessions", "PayCampaignId");
            CreateIndex("dbo.PaymentConcessions", "OwnerCampaignId");
            AddForeignKey("dbo.PaymentConcessions", "PayCampaignId", "dbo.Campaigns", "Id");
            AddForeignKey("dbo.PaymentConcessions", "OwnerCampaignId", "dbo.Campaigns", "Id");
        }
    }
}
