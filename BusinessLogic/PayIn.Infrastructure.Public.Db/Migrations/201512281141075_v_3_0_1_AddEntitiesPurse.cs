namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_3_0_1_AddEntitiesPurse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        NumberOfType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CampaignLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Min = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Max = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurseId = c.Int(nullable: false),
                        CampaignId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Purses", t => t.PurseId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .Index(t => t.PurseId)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.Purses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Validity = c.DateTime(nullable: false),
                        Expiration = c.DateTime(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
            AddColumn("dbo.PaymentConcessions", "OwnerCampaignId", c => c.Int());
            AddColumn("dbo.PaymentConcessions", "PayCampaignId", c => c.Int());
            AddColumn("dbo.PaymentMedias", "Balance", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PaymentMedias", "PurseId", c => c.Int());
            CreateIndex("dbo.PaymentConcessions", "OwnerCampaignId");
            CreateIndex("dbo.PaymentConcessions", "PayCampaignId");
            CreateIndex("dbo.PaymentMedias", "PurseId");
            AddForeignKey("dbo.PaymentMedias", "PurseId", "dbo.Purses", "Id");
            AddForeignKey("dbo.PaymentConcessions", "OwnerCampaignId", "dbo.Campaigns", "Id");
            AddForeignKey("dbo.PaymentConcessions", "PayCampaignId", "dbo.Campaigns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purses", "ConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.PaymentConcessions", "PayCampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.PaymentConcessions", "OwnerCampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.CampaignLines", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.PaymentMedias", "PurseId", "dbo.Purses");
            DropForeignKey("dbo.CampaignLines", "PurseId", "dbo.Purses");
            DropIndex("dbo.PaymentMedias", new[] { "PurseId" });
            DropIndex("dbo.Purses", new[] { "ConcessionId" });
            DropIndex("dbo.CampaignLines", new[] { "CampaignId" });
            DropIndex("dbo.CampaignLines", new[] { "PurseId" });
            DropIndex("dbo.PaymentConcessions", new[] { "PayCampaignId" });
            DropIndex("dbo.PaymentConcessions", new[] { "OwnerCampaignId" });
            DropColumn("dbo.PaymentMedias", "PurseId");
            DropColumn("dbo.PaymentMedias", "Balance");
            DropColumn("dbo.PaymentConcessions", "PayCampaignId");
            DropColumn("dbo.PaymentConcessions", "OwnerCampaignId");
            DropTable("dbo.Purses");
            DropTable("dbo.CampaignLines");
            DropTable("dbo.Campaigns");
        }
    }
}
