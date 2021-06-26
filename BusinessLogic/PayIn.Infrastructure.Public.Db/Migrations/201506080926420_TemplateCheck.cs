namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplateCheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlTemplateChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TemplateId = c.Int(nullable: false),
                        CheckPointId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCheckPoints", t => t.CheckPointId)
                .ForeignKey("dbo.ControlTemplates", t => t.TemplateId)
                .Index(t => t.TemplateId)
                .Index(t => t.CheckPointId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlTemplateChecks", "TemplateId", "dbo.ControlTemplates");
            DropForeignKey("dbo.ControlTemplateChecks", "CheckPointId", "dbo.ServiceCheckPoints");
            DropIndex("dbo.ControlTemplateChecks", new[] { "CheckPointId" });
            DropIndex("dbo.ControlTemplateChecks", new[] { "TemplateId" });
            DropTable("dbo.ControlTemplateChecks");
        }
    }
}
