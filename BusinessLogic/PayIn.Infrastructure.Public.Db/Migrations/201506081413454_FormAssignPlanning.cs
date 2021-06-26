namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormAssignPlanning : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlFormAssignPlannings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        CheckId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlPlanningChecks", t => t.CheckId)
                .ForeignKey("dbo.ControlForms", t => t.FormId)
                .Index(t => t.FormId)
                .Index(t => t.CheckId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlFormAssignPlannings", "FormId", "dbo.ControlForms");
            DropForeignKey("dbo.ControlFormAssignPlannings", "CheckId", "dbo.ControlPlanningChecks");
            DropIndex("dbo.ControlFormAssignPlannings", new[] { "CheckId" });
            DropIndex("dbo.ControlFormAssignPlannings", new[] { "FormId" });
            DropTable("dbo.ControlFormAssignPlannings");
        }
    }
}
