namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_RegenerandoTransportSimultaneousTitleCompatibility2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportSimultaneousTitleCompatibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransportTitleId = c.Int(nullable: false),
                        TransportTitle2Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportTitles", t => t.TransportTitleId)
                .ForeignKey("dbo.TransportTitles", t => t.TransportTitle2Id)
                .Index(t => t.TransportTitleId)
                .Index(t => t.TransportTitle2Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitle2Id", "dbo.TransportTitles");
            DropForeignKey("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitleId", "dbo.TransportTitles");
            DropIndex("dbo.TransportSimultaneousTitleCompatibilities", new[] { "TransportTitle2Id" });
            DropIndex("dbo.TransportSimultaneousTitleCompatibilities", new[] { "TransportTitleId" });
            DropTable("dbo.TransportSimultaneousTitleCompatibilities");
        }
    }
}
