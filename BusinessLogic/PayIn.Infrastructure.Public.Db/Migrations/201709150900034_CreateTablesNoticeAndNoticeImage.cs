namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTablesNoticeAndNoticeImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortDescription = c.String(),
                        Description = c.String(),
                        PhotoUrl = c.String(),
                        State = c.Int(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                        Visibility = c.Int(nullable: false),
                        EventId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.NoticeImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhotoUrl = c.String(nullable: false),
                        NoticeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notices", t => t.NoticeId)
                .Index(t => t.NoticeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notices", "EventId", "dbo.Events");
            DropForeignKey("dbo.NoticeImages", "NoticeId", "dbo.Notices");
            DropIndex("dbo.NoticeImages", new[] { "NoticeId" });
            DropIndex("dbo.Notices", new[] { "EventId" });
            DropTable("dbo.NoticeImages");
            DropTable("dbo.Notices");
        }
    }
}
