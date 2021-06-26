namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_AddNewClassesTransport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        OwnerCode = c.Int(nullable: false),
                        OwnerName = c.String(),
                        Type = c.Int(nullable: false),
                        SubType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransportCardTitleCompatibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransportTitleId = c.Int(nullable: false),
                        TransportCardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportCards", t => t.TransportCardId)
                .ForeignKey("dbo.TransportTitles", t => t.TransportTitleId)
                .Index(t => t.TransportTitleId)
                .Index(t => t.TransportCardId);
            
            CreateTable(
                "dbo.TransportTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        OwnerCode = c.Int(nullable: false),
                        OwnerName = c.String(nullable: false),
                        Zone = c.Int(nullable: false),
                        Image = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransportPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start_Value = c.DateTime(nullable: false),
                        End_Value = c.DateTime(nullable: false),
                        Version = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 5, scale: 2),
                        Quantity = c.Int(nullable: false),
                        TransportTitleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportTitles", t => t.TransportTitleId)
                .Index(t => t.TransportTitleId);
            
            CreateTable(
                "dbo.TransportSimultaneousTitleCompatibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransportTitleId = c.Int(nullable: false),
                        TransportTitleId2 = c.Int(nullable: false),
                        TransportTitle2_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportTitles", t => t.TransportTitleId)
                .ForeignKey("dbo.TransportTitles", t => t.TransportTitle2_Id)
                .Index(t => t.TransportTitleId)
                .Index(t => t.TransportTitle2_Id);
            
            CreateTable(
                "dbo.TransportSystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        cardType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitle2_Id", "dbo.TransportTitles");
            DropForeignKey("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitleId", "dbo.TransportTitles");
            DropForeignKey("dbo.TransportCardTitleCompatibilities", "TransportTitleId", "dbo.TransportTitles");
            DropForeignKey("dbo.TransportPrices", "TransportTitleId", "dbo.TransportTitles");
            DropForeignKey("dbo.TransportCardTitleCompatibilities", "TransportCardId", "dbo.TransportCards");
            DropIndex("dbo.TransportSimultaneousTitleCompatibilities", new[] { "TransportTitle2_Id" });
            DropIndex("dbo.TransportSimultaneousTitleCompatibilities", new[] { "TransportTitleId" });
            DropIndex("dbo.TransportPrices", new[] { "TransportTitleId" });
            DropIndex("dbo.TransportCardTitleCompatibilities", new[] { "TransportCardId" });
            DropIndex("dbo.TransportCardTitleCompatibilities", new[] { "TransportTitleId" });
            DropTable("dbo.TransportSystems");
            DropTable("dbo.TransportSimultaneousTitleCompatibilities");
            DropTable("dbo.TransportPrices");
            DropTable("dbo.TransportTitles");
            DropTable("dbo.TransportCardTitleCompatibilities");
            DropTable("dbo.TransportCards");
        }
    }
}
