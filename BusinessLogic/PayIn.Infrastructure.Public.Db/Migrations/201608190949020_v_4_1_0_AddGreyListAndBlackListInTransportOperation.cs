namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddGreyListAndBlackListInTransportOperation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportOperationBlackLists",
                c => new
                    {
                        TransportOperation_Id = c.Int(nullable: false),
                        BlackList_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TransportOperation_Id, t.BlackList_Id })
                .ForeignKey("dbo.TransportOperations", t => t.TransportOperation_Id, cascadeDelete: true)
                .ForeignKey("dbo.BlackLists", t => t.BlackList_Id, cascadeDelete: true)
                .Index(t => t.TransportOperation_Id)
                .Index(t => t.BlackList_Id);
            
            CreateTable(
                "dbo.GreyListTransportOperations",
                c => new
                    {
                        GreyList_Id = c.Int(nullable: false),
                        TransportOperation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GreyList_Id, t.TransportOperation_Id })
                .ForeignKey("dbo.GreyLists", t => t.GreyList_Id, cascadeDelete: true)
                .ForeignKey("dbo.TransportOperations", t => t.TransportOperation_Id, cascadeDelete: true)
                .Index(t => t.GreyList_Id)
                .Index(t => t.TransportOperation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GreyListTransportOperations", "TransportOperation_Id", "dbo.TransportOperations");
            DropForeignKey("dbo.GreyListTransportOperations", "GreyList_Id", "dbo.GreyLists");
            DropForeignKey("dbo.TransportOperationBlackLists", "BlackList_Id", "dbo.BlackLists");
            DropForeignKey("dbo.TransportOperationBlackLists", "TransportOperation_Id", "dbo.TransportOperations");
            DropIndex("dbo.GreyListTransportOperations", new[] { "TransportOperation_Id" });
            DropIndex("dbo.GreyListTransportOperations", new[] { "GreyList_Id" });
            DropIndex("dbo.TransportOperationBlackLists", new[] { "BlackList_Id" });
            DropIndex("dbo.TransportOperationBlackLists", new[] { "TransportOperation_Id" });
            DropTable("dbo.GreyListTransportOperations");
            DropTable("dbo.TransportOperationBlackLists");
        }
    }
}
