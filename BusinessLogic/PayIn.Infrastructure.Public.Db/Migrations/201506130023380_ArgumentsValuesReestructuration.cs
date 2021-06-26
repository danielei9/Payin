namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArgumentsValuesReestructuration : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ControlFormValues", name: "FormAssignPlanningId", newName: "PlanningId");
            RenameColumn(table: "dbo.ControlFormValues", name: "FormArgumentId", newName: "ArgumentId");
            RenameIndex(table: "dbo.ControlFormValues", name: "IX_FormAssignPlanningId", newName: "IX_PlanningId");
            RenameIndex(table: "dbo.ControlFormValues", name: "IX_FormArgumentId", newName: "IX_ArgumentId");
						RenameColumn(table: "dbo.ControlFormValues", name: "ValueDateTiem", newName: "ValueDateTime");
        }
        
        public override void Down()
        {
						RenameColumn(table: "dbo.ControlFormValues", name: "ValueDateTime", newName: "ValueDateTiem");
            RenameIndex(table: "dbo.ControlFormValues", name: "IX_ArgumentId", newName: "IX_FormArgumentId");
            RenameIndex(table: "dbo.ControlFormValues", name: "IX_PlanningId", newName: "IX_FormAssignPlanningId");
            RenameColumn(table: "dbo.ControlFormValues", name: "ArgumentId", newName: "FormArgumentId");
            RenameColumn(table: "dbo.ControlFormValues", name: "PlanningId", newName: "FormAssignPlanningId");
        }
    }
}
