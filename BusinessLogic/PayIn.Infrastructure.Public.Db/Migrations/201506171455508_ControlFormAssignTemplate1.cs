namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlFormAssignTemplate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlFormValues", "AssignTemplateId", "dbo.ControlFormAssignTemplates");
            DropIndex("dbo.ControlFormValues", new[] { "AssignTemplateId" });
            DropColumn("dbo.ControlFormValues", "AssignTemplateId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlFormValues", "AssignTemplateId", c => c.Int(nullable: false));
            CreateIndex("dbo.ControlFormValues", "AssignTemplateId");
            AddForeignKey("dbo.ControlFormValues", "AssignTemplateId", "dbo.ControlFormAssignTemplates", "Id");
        }
    }
}
