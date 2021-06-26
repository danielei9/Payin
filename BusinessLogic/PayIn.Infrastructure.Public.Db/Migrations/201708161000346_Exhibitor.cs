namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Exhibitor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exhibitors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        PaymentConcessionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentConcessions", t => t.PaymentConcessionId)
                .Index(t => t.PaymentConcessionId);
            
            CreateTable(
                "dbo.EventExhibitors",
                c => new
                    {
                        Event_Id = c.Int(nullable: false),
                        Exhibitor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Exhibitor_Id })
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.Exhibitors", t => t.Exhibitor_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.Exhibitor_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exhibitors", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropForeignKey("dbo.EventExhibitors", "Exhibitor_Id", "dbo.Exhibitors");
            DropForeignKey("dbo.EventExhibitors", "Event_Id", "dbo.Events");
            DropIndex("dbo.EventExhibitors", new[] { "Exhibitor_Id" });
            DropIndex("dbo.EventExhibitors", new[] { "Event_Id" });
            DropIndex("dbo.Exhibitors", new[] { "PaymentConcessionId" });
            DropTable("dbo.EventExhibitors");
            DropTable("dbo.Exhibitors");
        }
    }
}
