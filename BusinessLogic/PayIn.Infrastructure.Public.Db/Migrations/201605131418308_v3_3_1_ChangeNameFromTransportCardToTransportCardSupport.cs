namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_1_ChangeNameFromTransportCardToTransportCardSupport : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TransportCards", newName: "TransportCardSupports");
            DropForeignKey("dbo.TransportCardTitleCompatibilities", "TransportTitleId", "dbo.TransportTitles");
            DropForeignKey("dbo.TransportCardTitleCompatibilities", "TransportCardId", "dbo.TransportCards");
            DropIndex("dbo.TransportCardTitleCompatibilities", new[] { "TransportTitleId" });
            DropIndex("dbo.TransportCardTitleCompatibilities", new[] { "TransportCardId" });
            CreateTable(
                "dbo.TransportCardSupportTitleCompatibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransportTitleId = c.Int(nullable: false),
                        TransportCardSupportId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportTitles", t => t.TransportTitleId)
                .ForeignKey("dbo.TransportCardSupports", t => t.TransportCardSupportId)
                .Index(t => t.TransportTitleId)
                .Index(t => t.TransportCardSupportId);
            
            DropTable("dbo.TransportCardTitleCompatibilities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TransportCardTitleCompatibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransportTitleId = c.Int(nullable: false),
                        TransportCardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.TransportCardSupportTitleCompatibilities", "TransportCardSupportId", "dbo.TransportCardSupports");
            DropForeignKey("dbo.TransportCardSupportTitleCompatibilities", "TransportTitleId", "dbo.TransportTitles");
            DropIndex("dbo.TransportCardSupportTitleCompatibilities", new[] { "TransportCardSupportId" });
            DropIndex("dbo.TransportCardSupportTitleCompatibilities", new[] { "TransportTitleId" });
            DropTable("dbo.TransportCardSupportTitleCompatibilities");
            CreateIndex("dbo.TransportCardTitleCompatibilities", "TransportCardId");
            CreateIndex("dbo.TransportCardTitleCompatibilities", "TransportTitleId");
            AddForeignKey("dbo.TransportCardTitleCompatibilities", "TransportCardId", "dbo.TransportCards", "Id");
            AddForeignKey("dbo.TransportCardTitleCompatibilities", "TransportTitleId", "dbo.TransportTitles", "Id");
            RenameTable(name: "dbo.TransportCardSupports", newName: "TransportCards");
        }
    }
}
