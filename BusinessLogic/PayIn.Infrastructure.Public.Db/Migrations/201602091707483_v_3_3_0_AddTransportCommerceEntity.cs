namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_3_3_0_AddTransportCommerceEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransportCommerces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardType = c.Int(nullable: false),
                        UrlServer = c.String(nullable: false),
                        TransportType = c.Int(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportCommerces", "ConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.TransportCommerces", new[] { "ConcessionId" });
            DropTable("dbo.TransportCommerces");
        }
    }
}
