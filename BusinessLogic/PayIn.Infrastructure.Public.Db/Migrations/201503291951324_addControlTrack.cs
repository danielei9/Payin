namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addControlTrack : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlTracks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Latitude = c.Decimal(nullable: false, precision: 9, scale: 6),
                        Longitude = c.Decimal(nullable: false, precision: 9, scale: 6),
                        Quality = c.Int(nullable: false),
                        WorkerId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceWorkers", t => t.WorkerId)
                .ForeignKey("dbo.ControlItems", t => t.ItemId)
                .Index(t => t.WorkerId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlTracks", "ItemId", "dbo.ControlItems");
            DropForeignKey("dbo.ControlTracks", "WorkerId", "dbo.ServiceWorkers");
            DropIndex("dbo.ControlTracks", new[] { "ItemId" });
            DropIndex("dbo.ControlTracks", new[] { "WorkerId" });
            DropTable("dbo.ControlTracks");
        }
    }
}
