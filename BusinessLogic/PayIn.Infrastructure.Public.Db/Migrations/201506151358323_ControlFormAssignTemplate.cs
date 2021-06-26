namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlFormAssignTemplate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlFormAssignTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Decimal(precision: 9, scale: 6),
                        Longitude = c.Decimal(precision: 9, scale: 6),
                        FormId = c.Int(nullable: false),
                        CheckId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlTemplateChecks", t => t.CheckId)
                .ForeignKey("dbo.ControlForms", t => t.FormId)
                .Index(t => t.FormId)
                .Index(t => t.CheckId);
            
            AddColumn("dbo.ControlFormValues", "AssignTemplateId", c => c.Int(nullable: false));
            CreateIndex("dbo.ControlFormValues", "AssignTemplateId");
            AddForeignKey("dbo.ControlFormValues", "AssignTemplateId", "dbo.ControlFormAssignTemplates", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlFormAssignTemplates", "FormId", "dbo.ControlForms");
            DropForeignKey("dbo.ControlFormAssignTemplates", "CheckId", "dbo.ControlTemplateChecks");
            DropForeignKey("dbo.ControlFormValues", "AssignTemplateId", "dbo.ControlFormAssignTemplates");
            DropIndex("dbo.ControlFormAssignTemplates", new[] { "CheckId" });
            DropIndex("dbo.ControlFormAssignTemplates", new[] { "FormId" });
            DropIndex("dbo.ControlFormValues", new[] { "AssignTemplateId" });
            DropColumn("dbo.ControlFormValues", "AssignTemplateId");
            DropTable("dbo.ControlFormAssignTemplates");
        }
    }
}
