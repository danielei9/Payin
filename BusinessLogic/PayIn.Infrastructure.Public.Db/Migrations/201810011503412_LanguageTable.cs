namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LanguageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.Int(nullable: false),
                        Text = c.String(),
                        NoticeNameId = c.Int(),
                        NoticeDescriptionId = c.Int(),
                        NoticeShortDescriptionId = c.Int(),
                        NoticePlaceId = c.Int(),
                        EventNameId = c.Int(),
                        EventDescriptionId = c.Int(),
                        EventShortDescriptionId = c.Int(),
                        EventPlaceId = c.Int(),
                        EventConditionsId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notices", t => t.NoticeDescriptionId)
                .ForeignKey("dbo.Notices", t => t.NoticeNameId)
                .ForeignKey("dbo.Notices", t => t.NoticePlaceId)
                .ForeignKey("dbo.Notices", t => t.NoticeShortDescriptionId)
                .ForeignKey("dbo.Events", t => t.EventShortDescriptionId)
                .ForeignKey("dbo.Events", t => t.EventDescriptionId)
                .ForeignKey("dbo.Events", t => t.EventNameId)
                .ForeignKey("dbo.Events", t => t.EventPlaceId)
                .Index(t => t.NoticeNameId)
                .Index(t => t.NoticeDescriptionId)
                .Index(t => t.NoticeShortDescriptionId)
                .Index(t => t.NoticePlaceId)
                .Index(t => t.EventNameId)
                .Index(t => t.EventDescriptionId)
                .Index(t => t.EventShortDescriptionId)
                .Index(t => t.EventPlaceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Translations", "EventPlaceId", "dbo.Events");
            DropForeignKey("dbo.Translations", "EventNameId", "dbo.Events");
            DropForeignKey("dbo.Translations", "EventDescriptionId", "dbo.Events");
            DropForeignKey("dbo.Translations", "EventShortDescriptionId", "dbo.Events");
            DropForeignKey("dbo.Translations", "NoticeShortDescriptionId", "dbo.Notices");
            DropForeignKey("dbo.Translations", "NoticePlaceId", "dbo.Notices");
            DropForeignKey("dbo.Translations", "NoticeNameId", "dbo.Notices");
            DropForeignKey("dbo.Translations", "NoticeDescriptionId", "dbo.Notices");
            DropIndex("dbo.Translations", new[] { "EventPlaceId" });
            DropIndex("dbo.Translations", new[] { "EventShortDescriptionId" });
            DropIndex("dbo.Translations", new[] { "EventDescriptionId" });
            DropIndex("dbo.Translations", new[] { "EventNameId" });
            DropIndex("dbo.Translations", new[] { "NoticePlaceId" });
            DropIndex("dbo.Translations", new[] { "NoticeShortDescriptionId" });
            DropIndex("dbo.Translations", new[] { "NoticeDescriptionId" });
            DropIndex("dbo.Translations", new[] { "NoticeNameId" });
            DropTable("dbo.Translations");
        }
    }
}
