namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackReestructuration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlPresences", "WorkerId", "dbo.ServiceWorkers");
            DropForeignKey("dbo.ControlPresences", "ItemId", "dbo.ControlItems");
            DropIndex("dbo.ControlPresences", new[] { "WorkerId" });
            DropIndex("dbo.ControlPresences", new[] { "ItemId" });
            DropIndex("dbo.ControlTracks", new[] { "PresenceSinceId" });
            AlterColumn("dbo.ControlTracks", "PresenceSinceId", c => c.Int());
            CreateIndex("dbo.ControlTracks", "PresenceSinceId");
            DropColumn("dbo.ControlPresences", "PresenceType");
            DropColumn("dbo.ControlPresences", "WorkerId");
            DropColumn("dbo.ControlPresences", "ItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlPresences", "ItemId", c => c.Int(nullable: false));
            AddColumn("dbo.ControlPresences", "WorkerId", c => c.Int(nullable: false));
            AddColumn("dbo.ControlPresences", "PresenceType", c => c.Int());
            DropIndex("dbo.ControlTracks", new[] { "PresenceSinceId" });
            AlterColumn("dbo.ControlTracks", "PresenceSinceId", c => c.Int(nullable: false));
            CreateIndex("dbo.ControlTracks", "PresenceSinceId");
            CreateIndex("dbo.ControlPresences", "ItemId");
            CreateIndex("dbo.ControlPresences", "WorkerId");
            AddForeignKey("dbo.ControlPresences", "ItemId", "dbo.ControlItems", "Id");
            AddForeignKey("dbo.ControlPresences", "WorkerId", "dbo.ServiceWorkers", "Id");
        }
    }
}
