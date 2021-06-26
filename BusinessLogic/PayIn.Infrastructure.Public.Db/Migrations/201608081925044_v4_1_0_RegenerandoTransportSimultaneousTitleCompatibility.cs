namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_0_RegenerandoTransportSimultaneousTitleCompatibility : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitleId", "dbo.TransportTitles");
            DropForeignKey("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitle2_Id", "dbo.TransportTitles");
            DropIndex("dbo.TransportSimultaneousTitleCompatibilities", new[] { "TransportTitleId" });
            DropIndex("dbo.TransportSimultaneousTitleCompatibilities", new[] { "TransportTitle2_Id" });
            DropTable("dbo.TransportSimultaneousTitleCompatibilities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TransportSimultaneousTitleCompatibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransportTitleId = c.Int(nullable: false),
                        TransportTitleId2 = c.Int(),
                        TransportTitle2_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitle2_Id");
            CreateIndex("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitleId");
            AddForeignKey("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitle2_Id", "dbo.TransportTitles", "Id");
            AddForeignKey("dbo.TransportSimultaneousTitleCompatibilities", "TransportTitleId", "dbo.TransportTitles", "Id");
        }
    }
}
