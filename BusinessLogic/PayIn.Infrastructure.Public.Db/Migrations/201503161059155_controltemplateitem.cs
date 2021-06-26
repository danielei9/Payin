namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class controltemplateitem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlTemplateItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        TemplateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlTemplates", t => t.TemplateId)
                .Index(t => t.TemplateId);
            
            AddColumn("dbo.ControlTemplates", "Observation", c => c.String());
            DropColumn("dbo.ControlTemplates", "Since");
            DropColumn("dbo.ControlTemplates", "Until");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlTemplates", "Until", c => c.DateTime(nullable: false));
            AddColumn("dbo.ControlTemplates", "Since", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.ControlTemplateItems", "TemplateId", "dbo.ControlTemplates");
            DropIndex("dbo.ControlTemplateItems", new[] { "TemplateId" });
            DropColumn("dbo.ControlTemplates", "Observation");
            DropTable("dbo.ControlTemplateItems");
        }
    }
}
