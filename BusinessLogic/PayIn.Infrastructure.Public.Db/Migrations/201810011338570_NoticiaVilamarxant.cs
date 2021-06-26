namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoticiaVilamarxant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notices", "Longitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Notices", "Latitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Notices", "Place", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("dbo.Notices", "Type", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Notices", "End", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notices", "SuperNoticeId", c => c.Int());
            CreateIndex("dbo.Notices", "SuperNoticeId");
            AddForeignKey("dbo.Notices", "SuperNoticeId", "dbo.Notices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notices", "SuperNoticeId", "dbo.Notices");
            DropIndex("dbo.Notices", new[] { "SuperNoticeId" });
            DropColumn("dbo.Notices", "SuperNoticeId");
            DropColumn("dbo.Notices", "End");
            DropColumn("dbo.Notices", "Type");
            DropColumn("dbo.Notices", "Place");
            DropColumn("dbo.Notices", "Latitude");
            DropColumn("dbo.Notices", "Longitude");
        }
    }
}
