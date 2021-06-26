namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class controltemplate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        CheckDuration = c.Time(precision: 7),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlItems", t => t.ItemId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlTemplates", "ItemId", "dbo.ControlItems");
            DropIndex("dbo.ControlTemplates", new[] { "ItemId" });
            DropTable("dbo.ControlTemplates");
        }
    }
}
