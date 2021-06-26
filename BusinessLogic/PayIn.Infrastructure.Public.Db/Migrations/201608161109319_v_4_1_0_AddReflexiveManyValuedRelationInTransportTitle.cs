namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddReflexiveManyValuedRelationInTransportTitle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportTitleTransportTitles",
                c => new
                    {
                        TransportTitle_Id = c.Int(nullable: false),
                        TransportTitle_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TransportTitle_Id, t.TransportTitle_Id1 })
                .ForeignKey("dbo.TransportTitles", t => t.TransportTitle_Id)
                .ForeignKey("dbo.TransportTitles", t => t.TransportTitle_Id1)
                .Index(t => t.TransportTitle_Id)
                .Index(t => t.TransportTitle_Id1);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportTitleTransportTitles", "TransportTitle_Id1", "dbo.TransportTitles");
            DropForeignKey("dbo.TransportTitleTransportTitles", "TransportTitle_Id", "dbo.TransportTitles");
            DropIndex("dbo.TransportTitleTransportTitles", new[] { "TransportTitle_Id1" });
            DropIndex("dbo.TransportTitleTransportTitles", new[] { "TransportTitle_Id" });
            DropTable("dbo.TransportTitleTransportTitles");
        }
    }
}
