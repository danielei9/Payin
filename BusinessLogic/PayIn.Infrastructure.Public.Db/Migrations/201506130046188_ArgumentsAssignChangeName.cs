namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArgumentsAssignChangeName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ControlFormAssignPlannings", newName: "ControlFormAssigns");
            RenameColumn(table: "dbo.ControlFormValues", name: "PlanningId", newName: "AssignId");
            RenameIndex(table: "dbo.ControlFormValues", name: "IX_PlanningId", newName: "IX_AssignId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ControlFormValues", name: "IX_AssignId", newName: "IX_PlanningId");
            RenameColumn(table: "dbo.ControlFormValues", name: "AssignId", newName: "PlanningId");
            RenameTable(name: "dbo.ControlFormAssigns", newName: "ControlFormAssignPlannings");
        }
    }
}
