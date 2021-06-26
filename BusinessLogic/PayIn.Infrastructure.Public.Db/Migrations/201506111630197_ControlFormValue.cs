namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlFormValue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlFormValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Observations = c.String(),
                        Type = c.Int(nullable: false),
                        Target = c.Int(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        ValueString = c.String(),
                        ValueNumeric = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValueBool = c.Boolean(nullable: false),
                        ValueDateTiem = c.DateTime(nullable: false),
                        FormAssignPlanningId = c.Int(nullable: false),
                        FormArgumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlFormAssignPlannings", t => t.FormAssignPlanningId)
                .ForeignKey("dbo.ControlFormArguments", t => t.FormArgumentId)
                .Index(t => t.FormAssignPlanningId)
                .Index(t => t.FormArgumentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlFormValues", "FormArgumentId", "dbo.ControlFormArguments");
            DropForeignKey("dbo.ControlFormValues", "FormAssignPlanningId", "dbo.ControlFormAssignPlannings");
            DropIndex("dbo.ControlFormValues", new[] { "FormArgumentId" });
            DropIndex("dbo.ControlFormValues", new[] { "FormAssignPlanningId" });
            DropTable("dbo.ControlFormValues");
        }
    }
}
